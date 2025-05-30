export declare const SidebarItem: import("../utils").WithInstall<import("vue").DefineComponent<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    title: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    disabled: BooleanConstructor;
    badgeProps: import("vue").PropType<Partial<import("..").BadgeProps>>;
}, (() => import("vue/jsx-runtime").JSX.Element) | undefined, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "click"[], "click", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    to: import("vue").PropType<string | import("vue-router").RouteLocationAsRelativeGeneric | import("vue-router").RouteLocationAsPathGeneric>;
    url: StringConstructor;
    replace: BooleanConstructor;
} & {
    dot: BooleanConstructor;
    title: StringConstructor;
    badge: (NumberConstructor | StringConstructor)[];
    disabled: BooleanConstructor;
    badgeProps: import("vue").PropType<Partial<import("..").BadgeProps>>;
}>> & {
    onClick?: ((...args: any[]) => any) | undefined;
}, {
    replace: boolean;
    dot: boolean;
    disabled: boolean;
}, {}>>;
export default SidebarItem;
export { sidebarItemProps } from './SidebarItem';
export type { SidebarItemProps } from './SidebarItem';
export type { SidebarItemThemeVars } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanSidebarItem: typeof SidebarItem;
    }
}
