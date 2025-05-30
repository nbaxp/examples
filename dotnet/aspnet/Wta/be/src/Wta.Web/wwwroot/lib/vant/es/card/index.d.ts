export declare const Card: import("../utils").WithInstall<import("vue").DefineComponent<{
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
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "clickThumb"[], "clickThumb", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
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
}, {}>>;
export default Card;
export { cardProps } from './Card';
export type { CardProps } from './Card';
export type { CardThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanCard: typeof Card;
    }
}
