import { type PropType, type ExtractPropTypes } from 'vue';
import { type BadgeProps } from '../badge';
export declare const actionBarIconProps: {
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    color: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconClass: PropType<unknown>;
    badgeProps: PropType<Partial<BadgeProps>>;
    iconPrefix: StringConstructor;
};
export type ActionBarIconProps = ExtractPropTypes<typeof actionBarIconProps>;
declare const _default: import("vue").DefineComponent<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    color: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconClass: PropType<unknown>;
    badgeProps: PropType<Partial<BadgeProps>>;
    iconPrefix: StringConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    to: PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    color: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconClass: PropType<unknown>;
    badgeProps: PropType<Partial<BadgeProps>>;
    iconPrefix: StringConstructor;
}>>, {
    replace: boolean;
    dot: boolean;
}, {}>;
export default _default;
