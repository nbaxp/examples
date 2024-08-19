import { defineConfig } from 'vite';
import inspect from 'vite-plugin-inspect';

export default defineConfig({
  base: './',
  build: {
    target: 'esnext',
    module: 'esm',
  },
  plugins: [inspect()],
  server: {
    proxy: {
      '/api': {
        target: 'http://127.0.0.1:5000',
        changeOrigin: true,
      },
      '/api/hub': {
        target: 'ws://127.0.0.1:5000',
        ws: true,
      },
    },
  },
});
