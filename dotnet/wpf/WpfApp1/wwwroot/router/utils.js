import qs from "../lib/qs/shim.js";

function createDefaultRoute(type = "group", icon = "folder", isHidden = false, isTop = false) {
  return {
    type,
    icon,
    isTop,
    isHidden,
  };
}

function createRoute(path, meta = null, component = null) {
  return {
    path,
    component,
    meta: Object.assign(createDefaultRoute(), qs.parse(meta)),
  };
}

function createPage(path, meta = null, component = null) {
  return {
    path,
    component,
    meta: Object.assign(createDefaultRoute("page", "file"), qs.parse(meta)),
  };
}

function createButton(path, meta = null, show = null) {
  const result = {
    path,
    meta: Object.assign(createDefaultRoute("button", "file"), qs.parse(meta)),
  };
  if (show) {
    result.meta.show = show;
  }
  return result;
}

export default createRoute;
export { createPage, createButton };
