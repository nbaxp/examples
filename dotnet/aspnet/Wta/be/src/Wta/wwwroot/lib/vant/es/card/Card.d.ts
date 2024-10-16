import { type ExtractPropTypes } from 'vue';
export declare const cardProps: {
    tag: StringConstructor;
    num: (NumberConstructor | StringConstructor)[];
    desc: StringConstructor;
    thumb: StringConstructor;
    title: StringConstructor;
    price: (NumberConstructor | StringConstructor)[];
    centered: BooleanConstructor;
    lazyLoad: BooleanConstructor;
    currency: {
        type: import("vue").PropType<string>;
        default: string;
    };
    thumbLink: StringConstructor;
    originPrice: (NumberConstructor | StringConstructor)[];
};
export type CardProps = ExtractPropTypes<typeof cardProps>;
declare const _default: import("vue").DefineComponent<{
    tag: StringConstructor;
    num: (NumberConstructor | StringConstructor)[];
    desc: StringConstructor;
    thumb: StringConstructor;
    title: StringConstructor;
    price: (NumberConstructor | StringConstructor)[];
    centered: BooleanConstructor;
    lazyLoad: BooleanConstructor;
    currency: {
        type: import("vue").PropType<string>;
        default: string;
    };
    thumbLink: StringConstructor;
    originPrice: (NumberConstructor | StringConstructor)[];
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "clickThumb"[], "clickThumb", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    tag: StringConstructor;
    num: (NumberConstructor | StringConstructor)[];
    desc: StringConstructor;
    thumb: StringConstructor;
    title: StringConstructor;
    price: (NumberConstructor | StringConstructor)[];
    centered: BooleanConstructor;
    lazyLoad: BooleanConstructor;
    currency: {
        type: import("vue").PropType<string>;
        default: string;
    };
    thumbLink: StringConstructor;
    originPrice: (NumberConstructor | StringConstructor)[];
}>> & {
    onClickThumb?: ((...args: any[]) => any) | undefined;
}, {
    lazyLoad: boolean;
    centered: boolean;
    currency: string;
}, {}>;
export default _default;
