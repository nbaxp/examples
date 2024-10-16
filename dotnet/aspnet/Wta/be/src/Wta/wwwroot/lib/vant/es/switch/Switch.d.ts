import { type ExtractPropTypes } from 'vue';
export declare const switchProps: {
    size: (NumberConstructor | StringConstructor)[];
    loading: BooleanConstructor;
    disabled: BooleanConstructor;
    modelValue: import("vue").PropType<unknown>;
    activeColor: StringConstructor;
    inactiveColor: StringConstructor;
    activeValue: {
        type: import("vue").PropType<unknown>;
        default: unknown;
    };
    inactiveValue: {
        type: import("vue").PropType<unknown>;
        default: unknown;
    };
};
export type SwitchProps = ExtractPropTypes<typeof switchProps>;
declare const _default: import("vue").DefineComponent<{
    size: (NumberConstructor | StringConstructor)[];
    loading: BooleanConstructor;
    disabled: BooleanConstructor;
    modelValue: import("vue").PropType<unknown>;
    activeColor: StringConstructor;
    inactiveColor: StringConstructor;
    activeValue: {
        type: import("vue").PropType<unknown>;
        default: unknown;
    };
    inactiveValue: {
        type: import("vue").PropType<unknown>;
        default: unknown;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("update:modelValue" | "change")[], "update:modelValue" | "change", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    size: (NumberConstructor | StringConstructor)[];
    loading: BooleanConstructor;
    disabled: BooleanConstructor;
    modelValue: import("vue").PropType<unknown>;
    activeColor: StringConstructor;
    inactiveColor: StringConstructor;
    activeValue: {
        type: import("vue").PropType<unknown>;
        default: unknown;
    };
    inactiveValue: {
        type: import("vue").PropType<unknown>;
        default: unknown;
    };
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    "onUpdate:modelValue"?: ((...args: any[]) => any) | undefined;
}, {
    disabled: boolean;
    loading: boolean;
    activeValue: unknown;
    inactiveValue: unknown;
}, {}>;
export default _default;
