export declare const ActionBarIcon: import("../utils").WithInstall<import("vue").DefineComponent<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    color: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconClass: import("vue").PropType<unknown>;
    badgeProps: import("vue").PropType<Partial<import("..").BadgeProps>>;
    iconPrefix: StringConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    text: StringConstructor;
    icon: StringConstructor;
    color: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    iconClass: import("vue").PropType<unknown>;
    badgeProps: import("vue").PropType<Partial<import("..").BadgeProps>>;
    iconPrefix: StringConstructor;
}>>, {
    replace: boolean;
    dot: boolean;
}, {}>>;
export default ActionBarIcon;
export { actionBarIconProps } from './ActionBarIcon';
export type { ActionBarIconProps } from './ActionBarIcon';
export type { ActionBarIconThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanActionBarIcon: typeof ActionBarIcon;
    }
}
