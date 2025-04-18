import { type PropType, type ExtractPropTypes } from 'vue';
import { type BadgeProps } from '../badge';
export declare const gridItemProps: {
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconColor: StringConstructor;
    iconPrefix: StringConstructor;
    badgeProps: PropType<Partial<BadgeProps>>;
};
export type GridItemProps = ExtractPropTypes<typeof gridItemProps>;
declare const _default: import("vue").DefineComponent<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconColor: StringConstructor;
    iconPrefix: StringConstructor;
    badgeProps: PropType<Partial<BadgeProps>>;
}, (() => import("vue/jsx-runtime").JSX.Element) | undefined, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconColor: StringConstructor;
    iconPrefix: StringConstructor;
    badgeProps: PropType<Partial<BadgeProps>>;
}>>, {
    replace: boolean;
    dot: boolean;
}, {}>;
export default _default;
