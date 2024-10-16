import Mock from '~/lib/better-mock/mock.browser.esm.js';

function isVideoFile(file) {
  return ['mp4'].some((o) => file.endsWith(o));
}

export default function () {
  Mock.mock('/api/file/upload', 'POST', (request) => {
    const isVideo = Array.from(request.body.values()).some((o) => isVideoFile(o.name));
    return {
      code: 0,
      data: isVideo ? '/assets/test/video.mp4' : '/assets/logo.svg',
    };
  });
}
