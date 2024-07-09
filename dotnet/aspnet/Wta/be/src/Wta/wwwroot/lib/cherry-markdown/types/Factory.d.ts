export function createSyntaxHook(name: any, type: any, options: any): {
    new (editorConfig?: {}): {
        config: any;
        beforeMakeHtml(...args: any[]): any;
        makeHtml(...args: any[]): any;
        afterMakeHtml(...args: any[]): any;
        test(...args: any[]): any;
        rule(...args: any[]): any;
    };
    HOOK_NAME: any;
};
export function createMenuHook(name: any, options: any): {
    new (editorInstance: any): {
        noIcon: boolean;
        subMenuConfig: any;
        onClick(...args: any[]): any;
        readonly shortcutKeys: any;
        _onClick: (event?: MouseEvent | KeyboardEvent, shortKey?: string) => void;
        $cherry: Partial<import("./Cherry").default> & {
            $currentMenuOptions?: import("../types/menus").CustomMenuConfig;
        };
        bubbleMenu: boolean;
        subMenu: any;
        $currentMenuOptions: import("../types/menus").CustomMenuConfig;
        name: string;
        iconName: string;
        iconType: import("../types/menus").MenuIconType;
        editor: import("./Editor").default;
        locale: any;
        dom: HTMLSpanElement;
        updateMarkdown: boolean;
        cacheOnce: boolean;
        positionModel: "fixed" | "absolute" | "sidebar";
        fire(event?: MouseEvent | KeyboardEvent, shortKey?: string): void;
        getSubMenuConfig(): import("./toolbars/MenuBase").SubMenuConfigItem[];
        setName(name: string, iconName?: string): void;
        setCacheOnce(info: any): void;
        getAndCleanCacheOnce(): boolean;
        hasCacheOnce(): boolean;
        createIconFontIcon(iconName: string, options?: any): HTMLElement;
        createSvgIcon(options: import("../types/menus").CustomMenuIcon): Element;
        createImageIcon(options: import("../types/menus").CustomMenuIcon): HTMLImageElement;
        createBtn(asSubMenu?: boolean): HTMLSpanElement;
        createSubBtnByConfig(config: import("./toolbars/MenuBase").SubMenuConfigItem): HTMLSpanElement;
        isSelections: boolean;
        $getSelectionRange(): {
            begin: import("codemirror").Position;
            end: import("codemirror").Position;
        };
        registerAfterClickCb(cb: Function): void;
        afterClickCb: Function;
        $afterClick(): void;
        setLessSelection(lessBefore: string, lessAfter: string): void;
        getMoreSelection(appendBefore?: string, appendAfter?: string, cb?: Function): void;
        getSelection(selection: string, type?: string, focus?: boolean): string;
        bindSubClick(shortcut: any, selection: any): void;
        updateMenuIcon(options: string | HTMLElement | import("../types/menus").CustomMenuIcon): boolean;
        getMenuPosition(): Pick<DOMRect, "height" | "width" | "left" | "top">;
    };
    getTargetParentByButton(dom: HTMLElement): HTMLElement;
};
