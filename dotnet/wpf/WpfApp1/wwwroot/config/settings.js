export default {
  enableLocale: false,
  // baseURL: "http://dev.ccwin-in.com:16082/api",
  baseURL: new URLSearchParams(location.search).get("api") ?? "/api",
};
