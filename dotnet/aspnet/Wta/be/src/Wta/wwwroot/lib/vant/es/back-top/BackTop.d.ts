import { type PropType, type ExtractPropTypes } from 'vue';
export declare const backTopProps: {
    right: (NumberConstructor | StringConstructor)[];
    bottom: (NumberConstructor | StringConstructor)[];
    zIndex: (NumberConstructor | StringConstructor)[];
    target: PropType<string | import("vue").RendererElement | null | undefined>;
    offset: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    immediate: BooleanConstructor;
    teleport: {
        type: PropType<string | import("vue").RendererElement | null | undefined>;
        default: string;
    };
};
export type BackTopProps = ExtractPropTypes<typeof backTopProps>;
declare const _default: import("vue").DefineComponent<{
    right: (NumberConstructor | StringConstructor)[];
    bottom: (NumberConstructor | StringConstructor)[];
    zIndex: (NumberConstructor | StringConstructor)[];
    target: PropType<string | import("vue").RendererElement | null | undefined>;
    offset: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    immediate: BooleanConstructor;
    teleport: {
        type: PropType<string | import("vue").RendererElement | null | undefined>;
        default: string;
    };
}, () => import("vue/jsx-runtime").JSX.Element | import("vue/jsx-runtime").JSX.Element[], unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "click"[], "click", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    right: (NumberConstructor | StringConstructor)[];
    bottom: (NumberConstructor | StringConstructor)[];
    zIndex: (NumberConstructor | StringConstructor)[];
    target: PropType<string | import("vue").RendererElement | null | undefined>;
    offset: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    immediate: BooleanConstructor;
    teleport: {
        type: PropType<string | import("vue").RendererElement | null | undefined>;
        default: string;
    };
}>> & {
    onClick?: ((...args: any[]) => any) | undefined;
}, {
    offset: string | number;
    immediate: boolean;
    teleport: string | import("vue").RendererElement | null | undefined;
}, {}>;
export default _default;
