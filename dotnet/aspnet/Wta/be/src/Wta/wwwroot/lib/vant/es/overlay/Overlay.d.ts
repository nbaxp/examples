import { type PropType, type CSSProperties, type ExtractPropTypes } from 'vue';
export declare const overlayProps: {
    show: BooleanConstructor;
    zIndex: (NumberConstructor | StringConstructor)[];
    duration: (NumberConstructor | StringConstructor)[];
    className: PropType<unknown>;
    lockScroll: {
        type: BooleanConstructor;
        default: true;
    };
    lazyRender: {
        type: BooleanConstructor;
        default: true;
    };
    customStyle: PropType<CSSProperties>;
    teleport: PropType<string | import("vue").RendererElement | null | undefined>;
};
export type OverlayProps = ExtractPropTypes<typeof overlayProps>;
declare const _default: import("vue").DefineComponent<{
    show: BooleanConstructor;
    zIndex: (NumberConstructor | StringConstructor)[];
    duration: (NumberConstructor | StringConstructor)[];
    className: PropType<unknown>;
    lockScroll: {
        type: BooleanConstructor;
        default: true;
    };
    lazyRender: {
        type: BooleanConstructor;
        default: true;
    };
    customStyle: PropType<CSSProperties>;
    teleport: PropType<string | import("vue").RendererElement | null | undefined>;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    show: BooleanConstructor;
    zIndex: (NumberConstructor | StringConstructor)[];
    duration: (NumberConstructor | StringConstructor)[];
    className: PropType<unknown>;
    lockScroll: {
        type: BooleanConstructor;
        default: true;
    };
    lazyRender: {
        type: BooleanConstructor;
        default: true;
    };
    customStyle: PropType<CSSProperties>;
    teleport: PropType<string | import("vue").RendererElement | null | undefined>;
}>>, {
    show: boolean;
    lockScroll: boolean;
    lazyRender: boolean;
}, {}>;
export default _default;
