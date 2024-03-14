import qs from "../lib/qs/shim.js";
import { isLogin } from "../api/user.js";
import { useAppStore } from "../store/index.js";
import { getFileName } from "../utils/index.js";
import settings from "../config/settings.js";
import { ElMessage, ElMessageBox } from "element-plus";
import i18n from "../locale/index.js";

async function addToken(options) {
  if (await isLogin()) {
    const appStore = useAppStore();
    options.headers ??= {};
    options.headers.Authorization = `Bearer ${appStore.token}`;
  }
}

function getUrl(url) {
  if (url.startsWith("http")) {
    return url;
  }
  let result = settings.baseURL;
  return (result += `/${url}`);
}

async function getResult(response) {
  const messages = new Map([
    [200, "操作成功"],
    [201, "已创建"],
    [204, "无返回值"],
    [400, "请求参数错误"],
    [401, "未登录"],
    [415, "不支持的内容类型"],
    [403, "权限不足"],
    [500, "服务器异常"],
    [503, "服务不可用"],
  ]);
  const result = {
    status: response.status,
    message: messages.get(response.status),
  };
  const contentType = response.headers.get("Content-Type");
  if (response.status === 200 || response.status === 201 || response.status === 204) {
    if (contentType?.indexOf("application/json") > -1) {
      result.data = await response.json();
    } else if (contentType?.indexOf("text/plain") > -1) {
      result.data = await response.text();
    } else if (contentType === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
      result.data = await response.blob();
      result.filename = getFileName(response.headers.get("Content-Disposition"));
    }
  } else {
    try {
      if (contentType?.indexOf("application/json") > -1) {
        result.errors = await response.json();
      }
    } catch (error) {
      result.errors = error;
      console.log(error);
    }
    if (!result.errors) {
      result.errors = result.message ?? result.status;
    }
  }
  return result;
}

async function request(url, data, options, notify = false) {
  url = getUrl(url);
  let defaultOptions = {
    method: "POST",
    headers: {
      "Accept-Language": "zh-Hans",
    },
  };
  if (options) {
    Object.assign(defaultOptions, options);
  }
  if (defaultOptions.method === "GET" && data) {
    url = `${url}?${qs.stringify(data)}`;
  } else {
    if (defaultOptions.headers["Content-Type"]?.startsWith("application/x-www-form-urlencoded")) {
      defaultOptions.body = qs.stringify(data);
    } else if (defaultOptions.headers["Content-Type"]?.startsWith("application/json")) {
      defaultOptions.body = JSON.stringify(data);
    } else if (data instanceof FormData) {
      delete defaultOptions.headers["Content-Type"];
      defaultOptions.body = data;
    } else if (data) {
      defaultOptions.headers["Content-Type"] = "application/json";
      defaultOptions.body = JSON.stringify(data);
    }
  }
  await addToken(defaultOptions);
  const response = await fetch(url, defaultOptions);
  const result = await getResult(response);
  if (result.data?.code && result.data.code !== 200) {
    result.errors = result.data.message ?? result.data.code;
  }
  if (result.errors) {
    const key = String(result.errors?.error?.message ?? result.errors?.error?.code ?? result.errors ?? "错误");
    const message = i18n.global.t(key);
    ElMessageBox({ title: "提示", message, type: "warning" });
  } else if (notify && (!result.data?.code || result.data.code === 200)) {
    ElMessage({
      type: "success",
      message: "操作成功",
    });
  }
  return result;
}

async function get(url, data, notify = false) {
  return await request(url, data, { method: "GET" }, notify);
}

async function post(url, data, notify = false) {
  return await request(url, data, { method: "POST" }, notify);
}

export default request;
export { getUrl, get, post };
