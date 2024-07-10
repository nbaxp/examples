import schema from "@/models/settings.js";
import useAppStore from "@/store/app.js";
import AppForm from "@/views/components/form/index.js";
import { useClipboard, useMediaQuery } from "@vueuse/core";
import { ElMessage } from "element-plus";
import html from "utils";
import { ref, watchEffect } from "vue";

export default {
	components: { AppForm },
	template: html`<el-icon v-model="show" :size="18" class="cursor-pointer" @click="show=!show">
      <ep-setting />
    </el-icon>
    <el-drawer v-model="show" :title="$t('Page Settings')" append-to-body destroy-on-close size="auto">
      <app-form ref="formRef" :schema="schema" v-model="appStore" :hideButton="true" />
      <template #footer>
        <el-button type="primary" @click="copySettings">复制</el-button>
        <el-button type="primary" @click="reset">还原</el-button>
      </template>
    </el-drawer>`,
	setup() {
		const show = ref(false);
		const formRef = ref(null);
		const appStore = useAppStore();
		const { copy } = useClipboard();
		const copySettings = async () => {
			try {
				await formRef.value.validate();
				const text = JSON.stringify(appStore.$state, null, 2);
				await copy(text);
				ElMessage({
					message: "config/settings.json",
					type: "success",
				});
			} catch (error) {
				console.log(error);
			}
		};
		const reset = async () => {
			await formRef.value.reset();
		};
		watchEffect(() => {
			document.documentElement.classList[
				appStore.useDarkNav ? "add" : "remove"
			]("dark-nav");
		});
		watchEffect(() => {
			document.documentElement.style.setProperty(
				"--el-color-primary",
				appStore.color,
			);
		});
		watchEffect(() => {
			const darkClass = "dark";
			const toDark = () => document.documentElement.classList.add(darkClass);
			const toLight = () =>
				document.documentElement.classList.remove(darkClass);
			const isDarkNow = useMediaQuery("(prefers-color-scheme: dark)");
			if (appStore.mode === "auto") {
				isDarkNow.value ? toDark() : toLight();
			} else if (appStore.mode === "dark") {
				toDark();
			} else if (appStore.mode === "light") {
				toLight();
			}
		});
		return {
			show,
			formRef,
			schema,
			appStore,
			copySettings,
			reset,
		};
	},
};
