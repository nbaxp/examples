import { type PropType, type CSSProperties, type ExtractPropTypes } from 'vue';
export declare const tabProps: {
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    title: StringConstructor;
    disabled: BooleanConstructor;
    titleClass: PropType<unknown>;
    titleStyle: PropType<string | CSSProperties>;
    showZeroBadge: {
        type: BooleanConstructor;
        default: true;
    };
};
export type TabProps = ExtractPropTypes<typeof tabProps>;
declare const _default: import("vue").DefineComponent<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    title: StringConstructor;
    disabled: BooleanConstructor;
    titleClass: PropType<unknown>;
    titleStyle: PropType<string | CSSProperties>;
    showZeroBadge: {
        type: BooleanConstructor;
        default: true;
    };
}, (() => import("vue/jsx-runtime").JSX.Element | undefined) | undefined, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    title: StringConstructor;
    disabled: BooleanConstructor;
    titleClass: PropType<unknown>;
    titleStyle: PropType<string | CSSProperties>;
    showZeroBadge: {
        type: BooleanConstructor;
        default: true;
    };
}>>, {
    replace: boolean;
    dot: boolean;
    disabled: boolean;
    showZeroBadge: boolean;
}, {}>;
export default _default;
