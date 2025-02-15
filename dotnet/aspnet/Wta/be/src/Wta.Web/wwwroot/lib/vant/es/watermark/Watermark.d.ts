import { type ExtractPropTypes } from 'vue';
export declare const watermarkProps: {
    gapX: {
        type: NumberConstructor;
        default: number;
    };
    gapY: {
        type: NumberConstructor;
        default: number;
    };
    image: StringConstructor;
    width: {
        type: NumberConstructor;
        default: number;
    };
    height: {
        type: NumberConstructor;
        default: number;
    };
    rotate: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    zIndex: (NumberConstructor | StringConstructor)[];
    content: StringConstructor;
    opacity: (NumberConstructor | StringConstructor)[];
    fullPage: {
        type: BooleanConstructor;
        default: true;
    };
    textColor: {
        type: import("vue").PropType<string>;
        default: string;
    };
};
export type WatermarkProps = ExtractPropTypes<typeof watermarkProps>;
declare const _default: import("vue").DefineComponent<{
    gapX: {
        type: NumberConstructor;
        default: number;
    };
    gapY: {
        type: NumberConstructor;
        default: number;
    };
    image: StringConstructor;
    width: {
        type: NumberConstructor;
        default: number;
    };
    height: {
        type: NumberConstructor;
        default: number;
    };
    rotate: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    zIndex: (NumberConstructor | StringConstructor)[];
    content: StringConstructor;
    opacity: (NumberConstructor | StringConstructor)[];
    fullPage: {
        type: BooleanConstructor;
        default: true;
    };
    textColor: {
        type: import("vue").PropType<string>;
        default: string;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    gapX: {
        type: NumberConstructor;
        default: number;
    };
    gapY: {
        type: NumberConstructor;
        default: number;
    };
    image: StringConstructor;
    width: {
        type: NumberConstructor;
        default: number;
    };
    height: {
        type: NumberConstructor;
        default: number;
    };
    rotate: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    zIndex: (NumberConstructor | StringConstructor)[];
    content: StringConstructor;
    opacity: (NumberConstructor | StringConstructor)[];
    fullPage: {
        type: BooleanConstructor;
        default: true;
    };
    textColor: {
        type: import("vue").PropType<string>;
        default: string;
    };
}>>, {
    width: number;
    height: number;
    textColor: string;
    rotate: string | number;
    gapX: number;
    gapY: number;
    fullPage: boolean;
}, {}>;
export default _default;
