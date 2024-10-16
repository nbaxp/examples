import { ExtractPropTypes, PropType } from 'vue';
export type SpaceSize = number | string;
export type SpaceAlign = 'start' | 'end' | 'center' | 'baseline';
export declare const spaceProps: {
    align: PropType<SpaceAlign>;
    direction: {
        type: PropType<"horizontal" | "vertical">;
        default: string;
    };
    size: {
        type: PropType<string | number | [SpaceSize, SpaceSize]>;
        default: number;
    };
    wrap: BooleanConstructor;
    fill: BooleanConstructor;
};
export type SpaceProps = ExtractPropTypes<typeof spaceProps>;
declare const _default: import("vue").DefineComponent<{
    align: PropType<SpaceAlign>;
    direction: {
        type: PropType<"horizontal" | "vertical">;
        default: string;
    };
    size: {
        type: PropType<string | number | [SpaceSize, SpaceSize]>;
        default: number;
    };
    wrap: BooleanConstructor;
    fill: BooleanConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    align: PropType<SpaceAlign>;
    direction: {
        type: PropType<"horizontal" | "vertical">;
        default: string;
    };
    size: {
        type: PropType<string | number | [SpaceSize, SpaceSize]>;
        default: number;
    };
    wrap: BooleanConstructor;
    fill: BooleanConstructor;
}>>, {
    fill: boolean;
    size: string | number | [SpaceSize, SpaceSize];
    wrap: boolean;
    direction: "horizontal" | "vertical";
}, {}>;
export default _default;
