export declare const BackTop: import("../utils").WithInstall<import("vue").DefineComponent<{
    right: (NumberConstructor | StringConstructor)[];
    bottom: (NumberConstructor | StringConstructor)[];
    zIndex: (NumberConstructor | StringConstructor)[];
    target: import("vue").PropType<string | import("vue").RendererElement | null | undefined>;
    offset: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    immediate: BooleanConstructor;
    teleport: {
        type: import("vue").PropType<string | import("vue").RendererElement | null | undefined>;
        default: string;
    };
}, () => import("vue/jsx-runtime").JSX.Element | import("vue/jsx-runtime").JSX.Element[], unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "click"[], "click", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    right: (NumberConstructor | StringConstructor)[];
    bottom: (NumberConstructor | StringConstructor)[];
    zIndex: (NumberConstructor | StringConstructor)[];
    target: import("vue").PropType<string | import("vue").RendererElement | null | undefined>;
    offset: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    immediate: BooleanConstructor;
    teleport: {
        type: import("vue").PropType<string | import("vue").RendererElement | null | undefined>;
        default: string;
    };
}>> & {
    onClick?: ((...args: any[]) => any) | undefined;
}, {
    offset: string | number;
    immediate: boolean;
    teleport: string | import("vue").RendererElement | null | undefined;
}, {}>>;
export default BackTop;
export { backTopProps } from './BackTop';
export type { BackTopProps } from './BackTop';
export type { BackTopThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanBackTop: typeof BackTop;
    }
}
