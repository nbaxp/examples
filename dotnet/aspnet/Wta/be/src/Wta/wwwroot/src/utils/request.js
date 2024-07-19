import settings from '@/config/settings.js';
import i18n from '@/locale/index.js';
import useRouter from '@/router/index.js';
import { getFileNameFromContentDisposition } from 'utils';
import qs from '../../lib/qs/shim.js';
import { useTokenStore } from '../store/index.js';

const messages = new Map([
  [200, '操作成功'],
  [201, '已创建'],
  [204, '无返回值'],
  [301, '永久重定向'],
  [302, '临时重定向'],
  [400, '请求参数错误'],
  [401, '未登录'],
  [403, '权限不足'],
  [415, '不支持的内容类型'],
  [500, '服务器异常'],
  [503, '服务不可用'],
]);

/***/
function getUrl(url) {
  if (url.startsWith('http')) {
    return url;
  }
  return `${settings.baseURL}/${url}`;
}

async function getOptions(method, originalUrl, data, customOptions, isUrlEncoded) {
  let url = getUrl(originalUrl);
  //设置默认值
  const options = {
    //credentials: 'include',
    method: method ?? 'POST',
    headers: {
      'Accept-Language': i18n.global.locale.value,
    },
  };
  //合并自定义配置
  if (customOptions) {
    Object.assign(options, customOptions);
  }
  // 添加Token
  const tokenStore = useTokenStore();
  if (await tokenStore.isLogin()) {
    options.headers.Authorization = `Bearer ${tokenStore.accessToken}`;
  }
  if (options.method === 'GET') {
    //GET 拼接URL参数
    if (data) {
      if (data instanceof String) {
        url = `${url}?${data}`;
      } else {
        url = `${url}?${qs.stringify(data)}`;
      }
    }
  } else if (data instanceof FormData) {
    //上传参数
    //options.headers['Content-Type'] = 'application/json';
    options.body = data;
  } else {
    if (isUrlEncoded) {
      //urlencoded
      options.headers['Content-Type'] = 'application/x-www-form-urlencoded';
      options.body = qs.stringify(data);
    } else {
      //json
      options.headers['Content-Type'] = 'application/json';
      options.body = JSON.stringify(data);
    }
  }
  return {
    fullUrl: url,
    options,
  };
}

async function getResult(response) {
  const getJsonResult = async (response) => {
    const result = await response.json();
    if (result.code) {
      return result;
    }
    return {
      code: response.status,
      message: messages.get(response.status) ?? response.statusText,
      data: result,
    };
  };
  let result = null;
  if (response.status === 400) {
    //400输入错误
    result = await getJsonResult(response);
  } else if (response.status === 401) {
    //401未登录
    result = {
      code: response.status,
      message: messages.get(response.status) ?? response.statusText,
    };
    const router = await useRouter();
    router.push({
      path: '/login',
      query: { redirect: router.currentRoute.value.fullPath },
    });
  } else if (response.status === 403) {
    //403权限不足
    result = {
      code: response.status,
      message: messages.get(response.status) ?? response.statusText,
    };
    const router = await useRouter();
    router.push({
      path: '/403',
      query: { redirect: router.currentRoute.value.fullPath },
    });
  } else if (response.status === 500) {
    //500服务端错误
    result = await getJsonResult(response);
  } else {
    const contentType = response.headers.get('Content-Type');
    if (contentType?.indexOf('application/json') > -1) {
      result = await getJsonResult(response);
      if (result.data.error) {
        result.error = result.data.error;
        result.message = result.error.data.message;
      }
    } else {
      const contentDisposition = response.headers.get('Content-Disposition');
      if (contentDisposition) {
        result = {
          code: response.status,
          name: getFileNameFromContentDisposition(contentDisposition),
          data: await response.blob(),
        };
      } else {
        result = {
          code: response.status,
          message: messages.get(response.status) ?? response.statusText,
          data: await response.text(),
        };
      }
    }
  }
  if (!response.ok || result.code !== 200) {
    result.error = true;
    result.message ||= result.data ? result.data[''] : messages.get(result.code);
  }
  return result;
}

/**
 * @returns {code,message,data,error}
 */
async function request(method, url, data, customOptions, isUrlEncoded = false) {
  //规范化请求参数
  const { fullUrl, options } = await getOptions(method, url, data, customOptions, isUrlEncoded);
  try {
    //发送请求
    const response = await fetch(fullUrl, options);
    //规范化返回值格式
    const result = await getResult(response);
    return result;
  } catch (error) {
    return { code: error.name, message: error.message, error };
  }
}

export default request;
export { getUrl };
