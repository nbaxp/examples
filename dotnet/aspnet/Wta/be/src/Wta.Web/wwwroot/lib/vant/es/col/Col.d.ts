import { type ExtractPropTypes } from 'vue';
export declare const colProps: {
    tag: {
        type: import("vue").PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    span: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    offset: (NumberConstructor | StringConstructor)[];
};
export type ColProps = ExtractPropTypes<typeof colProps>;
declare const _default: import("vue").DefineComponent<{
    tag: {
        type: import("vue").PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    span: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    offset: (NumberConstructor | StringConstructor)[];
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    tag: {
        type: import("vue").PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    span: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    offset: (NumberConstructor | StringConstructor)[];
}>>, {
    span: string | number;
    tag: keyof HTMLElementTagNameMap;
}, {}>;
export default _default;
