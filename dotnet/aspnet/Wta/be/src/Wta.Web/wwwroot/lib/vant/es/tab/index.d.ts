export declare const Tab: import("../utils").WithInstall<import("vue").DefineComponent<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    title: StringConstructor;
    disabled: BooleanConstructor;
    titleClass: import("vue").PropType<unknown>;
    titleStyle: import("vue").PropType<string | import("vue").CSSProperties>;
    showZeroBadge: {
        type: BooleanConstructor;
        default: true;
    };
}, (() => import("vue/jsx-runtime").JSX.Element | undefined) | undefined, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    title: StringConstructor;
    disabled: BooleanConstructor;
    titleClass: import("vue").PropType<unknown>;
    titleStyle: import("vue").PropType<string | import("vue").CSSProperties>;
    showZeroBadge: {
        type: BooleanConstructor;
        default: true;
    };
}>>, {
    replace: boolean;
    dot: boolean;
    disabled: boolean;
    showZeroBadge: boolean;
}, {}>>;
export default Tab;
export { tabProps } from './Tab';
export type { TabProps } from './Tab';
declare module 'vue' {
    interface GlobalComponents {
        VanTab: typeof Tab;
    }
}
