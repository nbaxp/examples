import html from 'utils';
import { ref } from 'vue';
import '~/lib/zxing/zxing-browser.min.js';
import { showToast } from 'vant';

export default {
  template: html`
    <div style="text-align:center;background:#000;">
      <video id="webcamVideo" style="height:50vh;"></video>
    </div>
    <van-cell-group inset>
      <van-field v-model="code" label="编号" placeholder="扫码输入" clearable />
    </van-cell-group>
    <div>{{formats}}</div>
  `,
  setup() {
    const formats = ref([]);
    // const codeReader = new BrowserMultiFormatReader();
    // codeReader.listVideoInputDevices().then((devices) => {
    //   console.log(devices);
    // });
    let barcodeDetector = null;
    if ('BarcodeDetector' in globalThis) {
      alert('原生支持');
      BarcodeDetector.getSupportedFormats().then((supportedFormats) => {
        formats.value = supportedFormats;
        barcodeDetector = new BarcodeDetector({ formats: supportedFormats });
      });
    }
    const code = ref('');
    navigator.getUserMedia =
      navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
    if (navigator.getUserMedia) {
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
              if (barcodeDetector) {
                window.setInterval(async () => {
                  barcodeDetector.detect(video).then((barcodes) => {
                    code.value = barcodes[0].rawValue;
                    showToast(code.value);
                  });
                }, 1000);
              } else {
                var codeReader = new ZXingBrowser.BrowserMultiFormatReader();
                codeReader.decodeFromVideoElement(video, function (result) {
                  if (result) {
                    code.value = result.text;
                    showToast(code.value);
                  }
                });
              }
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
      formats,
      code,
    };
  },
};
