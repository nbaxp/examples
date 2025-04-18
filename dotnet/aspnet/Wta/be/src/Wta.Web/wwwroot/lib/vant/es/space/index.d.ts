export declare const Space: import("../utils").WithInstall<import("vue").DefineComponent<{
    align: import("vue").PropType<import("./Space").SpaceAlign>;
    direction: {
        type: import("vue").PropType<"horizontal" | "vertical">;
        default: string;
    };
    size: {
        type: import("vue").PropType<string | number | [import("./Space").SpaceSize, import("./Space").SpaceSize]>;
        default: number;
    };
    wrap: BooleanConstructor;
    fill: BooleanConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    align: import("vue").PropType<import("./Space").SpaceAlign>;
    direction: {
        type: import("vue").PropType<"horizontal" | "vertical">;
        default: string;
    };
    size: {
        type: import("vue").PropType<string | number | [import("./Space").SpaceSize, import("./Space").SpaceSize]>;
        default: number;
    };
    wrap: BooleanConstructor;
    fill: BooleanConstructor;
}>>, {
    fill: boolean;
    size: string | number | [import("./Space").SpaceSize, import("./Space").SpaceSize];
    wrap: boolean;
    direction: "horizontal" | "vertical";
}, {}>>;
export default Space;
export { spaceProps } from './Space';
export type { SpaceProps, SpaceSize, SpaceAlign } from './Space';
declare module 'vue' {
    interface GlobalComponents {
        VanSpace: typeof Space;
    }
}
