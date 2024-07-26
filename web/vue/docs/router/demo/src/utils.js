export function getFullPath(path) {
  if (!parent) {
    return path;
  }
  if (parent.path.endsWith("/")) {
    return parent.path + path;
  }
  return parent.path + "/" + path;
}
