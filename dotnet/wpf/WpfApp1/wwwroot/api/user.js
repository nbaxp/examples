import router from "../router/index.js";
import request from "../request/index.js";
import jwt_decode from "../lib/jwt-decode/jwt-decode.esm.js";
import { useAppStore } from "../store/index.js";
import Enumerable from "linq";
import { connection } from "../signalr/index.js";

const isLogin = async () => {
  const appStore = useAppStore();
  // 有 token，判断是否过期，失败设置 token 为 null
  if (appStore.token) {
    const exp = new Date(jwt_decode(appStore.token).exp * 1000);
    if (exp > new Date()) {
      return true;
    } else {
      appStore.token = null;
      removeAccessToken();
    }
  }
  return false;
};

const logout = () => {
  const appStore = useAppStore();
  appStore.token = null;
  removeAccessToken();
  removeRefreshToken();
  router.push({ path: "/login", query: { redirect: router.currentRoute.value.fullPath } });
};

const getUser = async () => {
  const result = await request("abp/application-configuration", null, {
    method: "GET",
  });
  const data = result.data;
  const user = {};
  user.id = data.currentUser.id;
  user.userName = data.currentUser.userName;
  user.email = data.currentUser.email;
  user.phoneNumber = data.currentUser.phoneNumber;
  user.roles = data.currentUser.roles;
  user.permissions = data.auth.policies;
  user.localization = data.localization;
  return user;
};

const hasPermission = (to) => {
  const appStore = useAppStore();
  const permission = to.meta?.permission;
  if (permission) {
    const hasPermission = Enumerable.from(appStore.user.permissions).any((o) => o.number === permission);
    return hasPermission;
  } else {
    return true;
  }
};

const accessTokenKey = "access_token";
const getAccessToken = () => localStorage.getItem(accessTokenKey);
const setAccessToken = (token) => localStorage.setItem(accessTokenKey, token);
const removeAccessToken = () => localStorage.removeItem(accessTokenKey);
const refreshTokenKey = "refresh_token";

const getRefreshToken = () => localStorage.getItem(refreshTokenKey);

const setRefreshToken = (refreshToken) => localStorage.setItem(refreshTokenKey, refreshToken);

const removeRefreshToken = () => {
  localStorage.removeItem(refreshTokenKey);
  connection.stop();
};

export { isLogin, logout, getAccessToken, setAccessToken, setRefreshToken, getUser, hasPermission };
