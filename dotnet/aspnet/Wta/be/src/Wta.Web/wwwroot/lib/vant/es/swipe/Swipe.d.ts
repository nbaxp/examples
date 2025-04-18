import { type ExtractPropTypes, type InjectionKey } from 'vue';
import { SwipeProvide } from './types';
export declare const swipeProps: {
    loop: {
        type: BooleanConstructor;
        default: true;
    };
    width: (NumberConstructor | StringConstructor)[];
    height: (NumberConstructor | StringConstructor)[];
    vertical: BooleanConstructor;
    autoplay: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    touchable: {
        type: BooleanConstructor;
        default: true;
    };
    lazyRender: BooleanConstructor;
    initialSwipe: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    indicatorColor: StringConstructor;
    showIndicators: {
        type: BooleanConstructor;
        default: true;
    };
    stopPropagation: {
        type: BooleanConstructor;
        default: true;
    };
};
export type SwipeProps = ExtractPropTypes<typeof swipeProps>;
export declare const SWIPE_KEY: InjectionKey<SwipeProvide>;
declare const _default: import("vue").DefineComponent<{
    loop: {
        type: BooleanConstructor;
        default: true;
    };
    width: (NumberConstructor | StringConstructor)[];
    height: (NumberConstructor | StringConstructor)[];
    vertical: BooleanConstructor;
    autoplay: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    touchable: {
        type: BooleanConstructor;
        default: true;
    };
    lazyRender: BooleanConstructor;
    initialSwipe: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    indicatorColor: StringConstructor;
    showIndicators: {
        type: BooleanConstructor;
        default: true;
    };
    stopPropagation: {
        type: BooleanConstructor;
        default: true;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("change" | "dragStart" | "dragEnd")[], "change" | "dragStart" | "dragEnd", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    loop: {
        type: BooleanConstructor;
        default: true;
    };
    width: (NumberConstructor | StringConstructor)[];
    height: (NumberConstructor | StringConstructor)[];
    vertical: BooleanConstructor;
    autoplay: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    touchable: {
        type: BooleanConstructor;
        default: true;
    };
    lazyRender: BooleanConstructor;
    initialSwipe: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    indicatorColor: StringConstructor;
    showIndicators: {
        type: BooleanConstructor;
        default: true;
    };
    stopPropagation: {
        type: BooleanConstructor;
        default: true;
    };
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    onDragStart?: ((...args: any[]) => any) | undefined;
    onDragEnd?: ((...args: any[]) => any) | undefined;
}, {
    autoplay: string | number;
    loop: boolean;
    vertical: boolean;
    duration: string | number;
    lazyRender: boolean;
    touchable: boolean;
    initialSwipe: string | number;
    showIndicators: boolean;
    stopPropagation: boolean;
}, {}>;
export default _default;
