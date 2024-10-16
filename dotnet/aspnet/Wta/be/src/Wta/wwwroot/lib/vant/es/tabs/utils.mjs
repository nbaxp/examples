import { raf, cancelRaf } from "@vant/use";
import { getScrollTop, setScrollTop } from "../utils/index.mjs";
function scrollLeftTo(scroller, to, duration) {
  let rafId;
  let count = 0;
  const from = scroller.scrollLeft;
  const frames = duration === 0 ? 1 : Math.round(duration * 1e3 / 16);
  let scrollLeft = from;
  function cancel() {
    cancelRaf(rafId);
  }
  function animate() {
    scrollLeft += (to - from) / frames;
    scroller.scrollLeft = scrollLeft;
    if (++count < frames) {
      rafId = raf(animate);
    }
  }
  animate();
  return cancel;
}
function scrollTopTo(scroller, to, duration, callback) {
  let rafId;
  let current = getScrollTop(scroller);
  const isDown = current < to;
  const frames = duration === 0 ? 1 : Math.round(duration * 1e3 / 16);
  const step = (to - current) / frames;
  function cancel() {
    cancelRaf(rafId);
  }
  function animate() {
    current += step;
    if (isDown && current > to || !isDown && current < to) {
      current = to;
    }
    setScrollTop(scroller, current);
    if (isDown && current < to || !isDown && current > to) {
      rafId = raf(animate);
    } else if (callback) {
      rafId = raf(callback);
    }
  }
  animate();
  return cancel;
}
export {
  scrollLeftTo,
  scrollTopTo
};
