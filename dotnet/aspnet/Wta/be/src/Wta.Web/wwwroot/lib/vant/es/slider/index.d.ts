export declare const Slider: import("../utils").WithInstall<import("vue").DefineComponent<{
    min: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    max: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    step: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    range: BooleanConstructor;
    reverse: BooleanConstructor;
    disabled: BooleanConstructor;
    readonly: BooleanConstructor;
    vertical: BooleanConstructor;
    barHeight: (NumberConstructor | StringConstructor)[];
    buttonSize: (NumberConstructor | StringConstructor)[];
    activeColor: StringConstructor;
    inactiveColor: StringConstructor;
    modelValue: {
        type: import("vue").PropType<number | [number, number]>;
        default: number;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("update:modelValue" | "change" | "dragStart" | "dragEnd")[], "update:modelValue" | "change" | "dragStart" | "dragEnd", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    min: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    max: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    step: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    range: BooleanConstructor;
    reverse: BooleanConstructor;
    disabled: BooleanConstructor;
    readonly: BooleanConstructor;
    vertical: BooleanConstructor;
    barHeight: (NumberConstructor | StringConstructor)[];
    buttonSize: (NumberConstructor | StringConstructor)[];
    activeColor: StringConstructor;
    inactiveColor: StringConstructor;
    modelValue: {
        type: import("vue").PropType<number | [number, number]>;
        default: number;
    };
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    "onUpdate:modelValue"?: ((...args: any[]) => any) | undefined;
    onDragStart?: ((...args: any[]) => any) | undefined;
    onDragEnd?: ((...args: any[]) => any) | undefined;
}, {
    reverse: boolean;
    range: boolean;
    max: string | number;
    disabled: boolean;
    vertical: boolean;
    min: string | number;
    modelValue: number | [number, number];
    readonly: boolean;
    step: string | number;
}, {}>>;
export default Slider;
export { sliderProps } from './Slider';
export type { SliderProps } from './Slider';
export type { SliderThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanSlider: typeof Slider;
    }
}
