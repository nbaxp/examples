export declare const Loading: import("../utils").WithInstall<import("vue").DefineComponent<{
    size: (NumberConstructor | StringConstructor)[];
    type: {
        type: import("vue").PropType<import("./Loading").LoadingType>;
        default: import("./Loading").LoadingType;
    };
    color: StringConstructor;
    vertical: BooleanConstructor;
    textSize: (NumberConstructor | StringConstructor)[];
    textColor: StringConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    size: (NumberConstructor | StringConstructor)[];
    type: {
        type: import("vue").PropType<import("./Loading").LoadingType>;
        default: import("./Loading").LoadingType;
    };
    color: StringConstructor;
    vertical: BooleanConstructor;
    textSize: (NumberConstructor | StringConstructor)[];
    textColor: StringConstructor;
}>>, {
    type: import("./Loading").LoadingType;
    vertical: boolean;
}, {}>>;
export default Loading;
export { loadingProps } from './Loading';
export type { LoadingType, LoadingProps } from './Loading';
export type { LoadingThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanLoading: typeof Loading;
    }
}
