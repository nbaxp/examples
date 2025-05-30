import { type PropType, type InjectionKey, type ExtractPropTypes, type ComponentPublicInstance } from 'vue';
import { type Numeric } from '../utils';
export type CollapseProvide = {
    toggle: (name: Numeric, expanded: boolean) => void;
    isExpanded: (name: Numeric) => boolean;
};
export type CollapseToggleAllOptions = boolean | {
    expanded?: boolean;
    skipDisabled?: boolean;
};
export declare const COLLAPSE_KEY: InjectionKey<CollapseProvide>;
export declare const collapseProps: {
    border: {
        type: BooleanConstructor;
        default: true;
    };
    accordion: BooleanConstructor;
    modelValue: {
        type: PropType<Numeric | Numeric[]>;
        default: string;
    };
};
export type CollapseProps = ExtractPropTypes<typeof collapseProps>;
export type CollapseInstance = ComponentPublicInstance<{
    toggleAll: (options?: boolean | CollapseToggleAllOptions) => void;
}>;
declare const _default: import("vue").DefineComponent<{
    border: {
        type: BooleanConstructor;
        default: true;
    };
    accordion: BooleanConstructor;
    modelValue: {
        type: PropType<Numeric | Numeric[]>;
        default: string;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("update:modelValue" | "change")[], "update:modelValue" | "change", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    border: {
        type: BooleanConstructor;
        default: true;
    };
    accordion: BooleanConstructor;
    modelValue: {
        type: PropType<Numeric | Numeric[]>;
        default: string;
    };
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    "onUpdate:modelValue"?: ((...args: any[]) => any) | undefined;
}, {
    border: boolean;
    modelValue: Numeric | Numeric[];
    accordion: boolean;
}, {}>;
export default _default;
