import { type ExtractPropTypes } from 'vue';
export interface BarrageItem {
    id: string | number;
    text: string | number;
}
export declare const barrageProps: {
    top: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    rows: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    autoPlay: {
        type: BooleanConstructor;
        default: true;
    };
    delay: {
        type: NumberConstructor;
        default: number;
    };
    modelValue: {
        type: import("vue").PropType<BarrageItem[]>;
        default: () => never[];
    };
};
export type BarrageProps = ExtractPropTypes<typeof barrageProps>;
declare const _default: import("vue").DefineComponent<{
    top: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    rows: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    autoPlay: {
        type: BooleanConstructor;
        default: true;
    };
    delay: {
        type: NumberConstructor;
        default: number;
    };
    modelValue: {
        type: import("vue").PropType<BarrageItem[]>;
        default: () => never[];
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "update:modelValue"[], "update:modelValue", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    top: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    rows: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    autoPlay: {
        type: BooleanConstructor;
        default: true;
    };
    delay: {
        type: NumberConstructor;
        default: number;
    };
    modelValue: {
        type: import("vue").PropType<BarrageItem[]>;
        default: () => never[];
    };
}>> & {
    "onUpdate:modelValue"?: ((...args: any[]) => any) | undefined;
}, {
    top: string | number;
    modelValue: BarrageItem[];
    rows: string | number;
    duration: string | number;
    autoPlay: boolean;
    delay: number;
}, {}>;
export default _default;
