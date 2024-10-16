export declare const TabbarItem: import("../utils").WithInstall<import("vue").DefineComponent<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    icon: StringConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    badgeProps: import("vue").PropType<Partial<import("..").BadgeProps>>;
    iconPrefix: StringConstructor;
}, (() => import("vue/jsx-runtime").JSX.Element) | undefined, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "click"[], "click", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    icon: StringConstructor;
    name: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    badgeProps: import("vue").PropType<Partial<import("..").BadgeProps>>;
    iconPrefix: StringConstructor;
}>> & {
    onClick?: ((...args: any[]) => any) | undefined;
}, {
    replace: boolean;
    dot: boolean;
}, {}>>;
export default TabbarItem;
export { tabbarItemProps } from './TabbarItem';
export type { TabbarItemProps } from './TabbarItem';
export type { TabbarItemThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanTabbarItem: typeof TabbarItem;
    }
}
