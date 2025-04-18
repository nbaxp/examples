import { type PropType, type ExtractPropTypes } from 'vue';
import { LoadingType } from '../loading';
import { ButtonSize, ButtonType, ButtonNativeType, ButtonIconPosition } from './types';
export declare const buttonProps: {
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    tag: {
        type: PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    text: StringConstructor;
    icon: StringConstructor;
    type: {
        type: PropType<ButtonType>;
        default: ButtonType;
    };
    size: {
        type: PropType<ButtonSize>;
        default: ButtonSize;
    };
    color: StringConstructor;
    block: BooleanConstructor;
    plain: BooleanConstructor;
    round: BooleanConstructor;
    square: BooleanConstructor;
    loading: BooleanConstructor;
    hairline: BooleanConstructor;
    disabled: BooleanConstructor;
    iconPrefix: StringConstructor;
    nativeType: {
        type: PropType<ButtonNativeType>;
        default: ButtonNativeType;
    };
    loadingSize: (NumberConstructor | StringConstructor)[];
    loadingText: StringConstructor;
    loadingType: PropType<LoadingType>;
    iconPosition: {
        type: PropType<ButtonIconPosition>;
        default: ButtonIconPosition;
    };
};
export type ButtonProps = ExtractPropTypes<typeof buttonProps>;
declare const _default: import("vue").DefineComponent<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    tag: {
        type: PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    text: StringConstructor;
    icon: StringConstructor;
    type: {
        type: PropType<ButtonType>;
        default: ButtonType;
    };
    size: {
        type: PropType<ButtonSize>;
        default: ButtonSize;
    };
    color: StringConstructor;
    block: BooleanConstructor;
    plain: BooleanConstructor;
    round: BooleanConstructor;
    square: BooleanConstructor;
    loading: BooleanConstructor;
    hairline: BooleanConstructor;
    disabled: BooleanConstructor;
    iconPrefix: StringConstructor;
    nativeType: {
        type: PropType<ButtonNativeType>;
        default: ButtonNativeType;
    };
    loadingSize: (NumberConstructor | StringConstructor)[];
    loadingText: StringConstructor;
    loadingType: PropType<LoadingType>;
    iconPosition: {
        type: PropType<ButtonIconPosition>;
        default: ButtonIconPosition;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "click"[], "click", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    tag: {
        type: PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    text: StringConstructor;
    icon: StringConstructor;
    type: {
        type: PropType<ButtonType>;
        default: ButtonType;
    };
    size: {
        type: PropType<ButtonSize>;
        default: ButtonSize;
    };
    color: StringConstructor;
    block: BooleanConstructor;
    plain: BooleanConstructor;
    round: BooleanConstructor;
    square: BooleanConstructor;
    loading: BooleanConstructor;
    hairline: BooleanConstructor;
    disabled: BooleanConstructor;
    iconPrefix: StringConstructor;
    nativeType: {
        type: PropType<ButtonNativeType>;
        default: ButtonNativeType;
    };
    loadingSize: (NumberConstructor | StringConstructor)[];
    loadingText: StringConstructor;
    loadingType: PropType<LoadingType>;
    iconPosition: {
        type: PropType<ButtonIconPosition>;
        default: ButtonIconPosition;
    };
}>> & {
    onClick?: ((...args: any[]) => any) | undefined;
}, {
    replace: boolean;
    type: ButtonType;
    tag: keyof HTMLElementTagNameMap;
    round: boolean;
    size: ButtonSize;
    disabled: boolean;
    block: boolean;
    square: boolean;
    loading: boolean;
    plain: boolean;
    hairline: boolean;
    nativeType: ButtonNativeType;
    iconPosition: ButtonIconPosition;
}, {}>;
export default _default;
