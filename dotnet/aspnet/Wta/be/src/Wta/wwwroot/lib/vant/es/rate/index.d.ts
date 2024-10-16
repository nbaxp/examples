export declare const Rate: import("../utils").WithInstall<import("vue").DefineComponent<{
    size: (NumberConstructor | StringConstructor)[];
    icon: {
        type: import("vue").PropType<string>;
        default: string;
    };
    color: StringConstructor;
    count: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    gutter: (NumberConstructor | StringConstructor)[];
    clearable: BooleanConstructor;
    readonly: BooleanConstructor;
    disabled: BooleanConstructor;
    voidIcon: {
        type: import("vue").PropType<string>;
        default: string;
    };
    allowHalf: BooleanConstructor;
    voidColor: StringConstructor;
    touchable: {
        type: BooleanConstructor;
        default: true;
    };
    iconPrefix: StringConstructor;
    modelValue: {
        type: NumberConstructor;
        default: number;
    };
    disabledColor: StringConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("update:modelValue" | "change")[], "update:modelValue" | "change", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    size: (NumberConstructor | StringConstructor)[];
    icon: {
        type: import("vue").PropType<string>;
        default: string;
    };
    color: StringConstructor;
    count: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    gutter: (NumberConstructor | StringConstructor)[];
    clearable: BooleanConstructor;
    readonly: BooleanConstructor;
    disabled: BooleanConstructor;
    voidIcon: {
        type: import("vue").PropType<string>;
        default: string;
    };
    allowHalf: BooleanConstructor;
    voidColor: StringConstructor;
    touchable: {
        type: BooleanConstructor;
        default: true;
    };
    iconPrefix: StringConstructor;
    modelValue: {
        type: NumberConstructor;
        default: number;
    };
    disabledColor: StringConstructor;
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    "onUpdate:modelValue"?: ((...args: any[]) => any) | undefined;
}, {
    disabled: boolean;
    icon: string;
    clearable: boolean;
    modelValue: number;
    readonly: boolean;
    touchable: boolean;
    count: string | number;
    voidIcon: string;
    allowHalf: boolean;
}, {}>>;
export default Rate;
export { rateProps } from './Rate';
export type { RateProps } from './Rate';
export type { RateThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanRate: typeof Rate;
    }
}
