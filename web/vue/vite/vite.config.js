import vue from '@vitejs/plugin-vue';
import vueJsx from '@vitejs/plugin-vue-jsx';
import { presetAttributify, presetIcons, presetUno, transformerDirectives, transformerVariantGroup } from 'unocss';
import UnoCSS from 'unocss/vite';
import { ElementPlusResolver } from 'unplugin-vue-components/resolvers';
import components from 'unplugin-vue-components/vite';
import { defineConfig } from 'vite';
import viteCompression from 'vite-plugin-compression';
import inspect from 'vite-plugin-inspect';
import svgLoader from 'vite-svg-loader';

// https://icones.js.org/collection/all

export default defineConfig({
  base: '/',
  build: {
    target: 'esnext',
    chunkSizeWarningLimit: 2000,
  },
  resolve: {
    alias: {
      '@/': new URL('./src/', import.meta.url).pathname,
      vue: 'vue/dist/vue.esm-bundler.js',
    },
  },
  plugins: [
    vue(),
    vueJsx(),
    inspect(),
    svgLoader(),
    viteCompression(),
    components({
      extensions: ['vue', 'md'],
      include: [/\.vue$/, /\.vue\?vue/, /\.md$/],
      resolvers: [ElementPlusResolver()],
    }),
    UnoCSS({
      presets: [
        presetUno(),
        presetAttributify(),
        presetIcons({
          scale: 1.2,
          warn: true,
        }),
      ],
      transformers: [transformerDirectives(), transformerVariantGroup()],
    }),
  ],
});