import copy from 'rollup-plugin-copy';
import { defineConfig } from 'vite';
import inspect from 'vite-plugin-inspect';
import viteRestart from 'vite-plugin-restart';

const path = require('path');
const fs = require('fs');

const replaceImportMap = (html) => {
  return html.replace(/<script[^<>]+importmap[^>]+>[^<>]+<\/script>\n*/i, '');
};

const transformHtml = () => {
  return {
    name: 'html-transform',
    transformIndexHtml(html) {
      return replaceImportMap(html);
    },
  };
};

const getImports = () => {
  const file = path.resolve(process.cwd(), 'index.html');
  const html = fs.readFileSync(file, 'utf-8');
  const importmapContent = html.match(/<script[^<>]+importmap[^>]+>([^<>]+)<\/script>\n*/)[1];
  const importmap = JSON.parse(importmapContent);
  console.log('importmap:');
  console.log(importmap);
  return importmap;
};

const alias = {};
Object.assign(alias, getImports().imports);

export default defineConfig({
  base: './',
  build: {
    target: 'esnext',
    module: 'esm',
  },
  resolve: {
    alias,
  },
  plugins: [
    inspect(),
    copy({
      targets: [
        {
          src: 'dist/index.html',
          dest: 'dist',
          transform: (contents) => {
            return replaceImportMap(contents.toString());
          },
        },
        { src: 'assets', dest: 'dist' },
      ],
      hook: 'writeBundle',
    }),
    transformHtml(),
    viteRestart({
      restart: ['./index.html'],
    }),
  ],
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
