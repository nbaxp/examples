import type { ComponentPublicInstance } from 'vue';
import type { CollapseItemProps } from './CollapseItem';
export type CollapseItemExpose = {
    toggle: (newValue?: boolean) => void;
};
export type CollapseItemInstance = ComponentPublicInstance<CollapseItemProps, CollapseItemExpose>;
export type CollapseItemThemeVars = {
    collapseItemDuration?: string;
    collapseItemContentPadding?: string;
    collapseItemContentFontSize?: string;
    collapseItemContentLineHeight?: number | string;
    collapseItemContentTextColor?: string;
    collapseItemContentBackground?: string;
    collapseItemTitleDisabledColor?: string;
};
