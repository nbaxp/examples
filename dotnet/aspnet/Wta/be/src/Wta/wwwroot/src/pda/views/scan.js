import html from 'utils';
import { ref } from 'vue';
import '~/lib/zxing/zxing-browser.min.js';

export default {
  template: html`
    <div style="text-align:center;background:#000;">
      <video id="webcamVideo" style="height:50vh;"></video>
    </div>
    <van-cell-group inset>
      <van-field v-model="code" label="编号" placeholder="扫码输入" clearable />
    </van-cell-group>
  `,
  setup() {
    // const codeReader = new BrowserMultiFormatReader();
    // codeReader.listVideoInputDevices().then((devices) => {
    //   console.log(devices);
    // });
    const code = ref('');
    navigator.getUserMedia =
      navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
    if (navigator.getUserMedia) {
      var codeReader = new ZXingBrowser.BrowserMultiFormatReader();
      navigator.getUserMedia(
        {
          audio: false,
          video: {
            facingMode: 'environment',
          },
        },
        function (stream) {
          var video = document.getElementById('webcamVideo');
          video.srcObject = stream;
          video.addEventListener('loadedmetadata', function () {
            video.play().then(function () {
              codeReader.decodeFromVideoElement(video, function (result) {
                if (result) {
                  code.value = result.text;
                }
              });
            });
          });
        },
        function (error) {
          alert('无法打开摄像头');
          console.log('Error:', error);
        },
      );
    }
    return {
      code,
    };
  },
};
