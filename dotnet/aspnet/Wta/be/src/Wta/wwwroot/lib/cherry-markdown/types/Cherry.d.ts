/** @typedef {import('../../types/cherry').CherryOptions} CherryOptions */
export default class Cherry extends CherryStatic {
    /**
     * @protected
     */
    protected static initialized: boolean;
    /**
     * @readonly
     */
    static readonly config: {
        /** @type {Partial<CherryOptions>} */
        defaults: Partial<CherryOptions>;
    };
    /**
     * @param {Partial<CherryOptions>} options
     */
    constructor(options: Partial<CherryOptions>);
    defaultToolbar: false | (string | import("../../types/cherry").CherryInsertToolbar)[];
    /**
     * @property
     * @type {Partial<CherryOptions>}
     */
    options: Partial<CherryOptions>;
    locales: {
        zh_CN: {
            bold: string;
            code: string;
            graph: string;
            h1: string;
            h2: string;
            h3: string;
            h4: string;
            h5: string;
            header: string;
            insert: string;
            italic: string;
            list: string;
            quickTable: string;
            quote: string;
            size: string;
            color: string;
            strikethrough: string;
            sub: string;
            sup: string;
            togglePreview: string;
            fullScreen: string;
            image: string;
            audio: string;
            video: string;
            link: string;
            hr: string;
            br: string;
            toc: string;
            pdf: string;
            word: string;
            table: string;
            'line-table': string;
            'bar-table': string;
            formula: string;
            insertFormula: string;
            insertFlow: string;
            insertSeq: string;
            insertState: string;
            insertClass: string;
            insertPie: string;
            insertGantt: string;
            checklist: string;
            ol: string;
            ul: string;
            undo: string;
            redo: string;
            previewClose: string;
            codeTheme: string;
            switchModel: string;
            switchPreview: string;
            switchEdit: string;
            classicBr: string;
            normalBr: string;
            settings: string;
            mobilePreview: string;
            copy: string;
            export: string;
            underline: string;
            pinyin: string;
            file: string;
            pastePlain: string;
            pasteMarkdown: string; /**
             * @property
             * @type {Partial<CherryOptions>}
             */
            hide: string;
            exportToPdf: string;
            exportScreenshot: string;
            exportMarkdownFile: string;
            exportHTMLFile: string;
            theme: string;
            panel: string;
            detail: string;
            'H1 Heading': string;
            'H2 Heading': string;
            'H3 Heading': string;
            complement: string;
            summary: string;
            justify: string;
            justifyLeft: string;
            justifyCenter: string;
            justifyRight: string;
            publish: string;
            wordCount: string;
            fontColor: string;
            fontBgColor: string;
            small: string;
            medium: string;
            large: string;
            superLarge: string;
            detailDefaultContent: string;
        };
        en_US: {
            bold: string;
            code: string;
            graph: string;
            h1: string;
            h2: string;
            h3: string;
            h4: string;
            h5: string;
            header: string;
            insert: string;
            italic: string;
            list: string;
            quickTable: string;
            quote: string;
            size: string;
            color: string;
            strikethrough: string;
            sub: string;
            sup: string;
            togglePreview: string;
            fullScreen: string;
            image: string;
            audio: string;
            video: string;
            link: string;
            hr: string;
            br: string;
            toc: string;
            pdf: string;
            word: string;
            table: string;
            'line-table': string;
            'bar-table': string;
            formula: string;
            insertFormula: string;
            insertFlow: string;
            insertSeq: string;
            insertState: string;
            insertClass: string;
            insertPie: string;
            insertGantt: string;
            checklist: string;
            ol: string;
            ul: string;
            undo: string;
            redo: string;
            previewClose: string;
            codeTheme: string;
            switchModel: string;
            switchPreview: string;
            switchEdit: string;
            classicBr: string;
            normalBr: string;
            settings: string;
            mobilePreview: string;
            copy: string;
            export: string;
            underline: string;
            pinyin: string;
            pastePlain: string;
            pasteMarkdown: string;
            hide: string;
            exportToPdf: string;
            exportScreenshot: string;
            exportMarkdownFile: string;
            exportHTMLFile: string;
            'H1 Heading': string;
            'H2 Heading': string;
            'H3 Heading': string;
            complement: string;
            summary: string;
            justify: string;
            justifyLeft: string;
            justifyCenter: string;
            justifyRight: string;
            publish: string;
            wordCount: string;
            fontColor: string;
            fontBgColor: string;
            small: string;
            medium: string;
            large: string;
            superLarge: string;
            detailDefaultContent: string;
        };
    };
    locale: any;
    status: {
        toolbar: string;
        previewer: string;
        editor: string;
    };
    /**
     * @property
     * @type {string} 实例ID
     */
    instanceId: string;
    lastMarkdownText: string;
    $event: Event;
    /**
     * @type {import('./Engine').default}
     */
    engine: import('./Engine').default;
    /**
     * 初始化工具栏、编辑区、预览区等
     * @private
     */
    private init;
    noMountEl: boolean;
    cherryDom: HTMLElement;
    wrapperDom: HTMLDivElement;
    destroy(): void;
    on(eventName: any, callback: any): void;
    off(eventName: any, callback: any): void;
    createToc(): void;
    toc: Toc;
    /**
     * 滚动到hash位置，实际上就是通过修改location.hash来触发hashChange事件，剩下的就交给浏览器了
     */
    scrollByHash(): void;
    $t(str: any): any;
    addLocale(key: any, value: any): void;
    addLocales(locales: any): void;
    getLocales(): any;
    /**
     * 切换编辑模式
     * @param {'edit&preview'|'editOnly'|'previewOnly'} [model=edit&preview] 模式类型
     * 一般纯预览模式和纯编辑模式适合在屏幕较小的终端使用，比如手机移动端
     */
    switchModel(model?: 'edit&preview' | 'editOnly' | 'previewOnly', showToolbar?: boolean): void;
    /**
     * 获取实例id
     * @returns {string}
     * @public
     */
    public getInstanceId(): string;
    /**
     * 获取编辑器状态
     * @returns  {Object}
     */
    getStatus(): any;
    /**
     * 获取编辑区内的markdown源码内容
     * @returns markdown源码内容
     */
    getValue(): string;
    /**
     * 获取编辑区内的markdown源码内容
     * @returns {string} markdown源码内容
     */
    getMarkdown(): string;
    /**
     * 获取CodeMirror 实例
     * @returns { CodeMirror.Editor } CodeMirror实例
     */
    getCodeMirror(): CodeMirror.Editor;
    /**
     * 获取预览区内的html内容
     * @param {boolean} [wrapTheme=true] 是否在外层包裹主题class
     * @returns {string} html内容
     */
    getHtml(wrapTheme?: boolean): string;
    /**
     * 获取Previewer 预览实例
     * @returns {Previewer} Previewer 预览实例
     */
    getPreviewer(): Previewer;
    /**
     * @typedef {{
     *  level: number;
     * id: string;
     * text: string;
     * }[]} HeaderList
     * 获取目录，目录由head1~6组成
     * @returns {HeaderList} 标题head数组
     */
    getToc(): {
        level: number;
        id: string;
        text: string;
    }[];
    /**
     * 覆盖编辑区的内容
     * @param {string} content markdown内容
     * @param {boolean} keepCursor 是否保持光标位置
     */
    setValue(content: string, keepCursor?: boolean): void;
    /**
     * 在光标处或者指定行+偏移量插入内容
     * @param {string} content 被插入的文本
     * @param {boolean} [isSelect=false] 是否选中刚插入的内容
     * @param {[number, number]|false} [anchor=false] [x,y] 代表x+1行，y+1字符偏移量，默认false 会从光标处插入
     * @param {boolean} [focus=true] 保持编辑器处于focus状态
     */
    insert(content: string, isSelect?: boolean, anchor?: [number, number] | false, focus?: boolean): void;
    /**
     * 在光标处或者指定行+偏移量插入内容
     * @param {string} content 被插入的文本
     * @param {boolean} [isSelect=false] 是否选中刚插入的内容
     * @param {[number, number]|false} [anchor=false] [x,y] 代表x+1行，y+1字符偏移量，默认false 会从光标处插入
     * @param {boolean} [focus=true] 保持编辑器处于focus状态
     * @returns
     */
    insertValue(content: string, isSelect?: boolean, anchor?: [number, number] | false, focus?: boolean): void;
    /**
     * 强制重新渲染预览区域
     */
    refreshPreviewer(): void;
    /**
     * 覆盖编辑区的内容
     * @param {string} content markdown内容
     * @param {boolean} [keepCursor=false] 是否保持光标位置
     */
    setMarkdown(content: string, keepCursor?: boolean): void;
    /**
     * @private
     * @returns
     */
    private createWrapper;
    /**
     * @private
     * @returns {Toolbar}
     */
    private createToolbar;
    toolbarContainer: HTMLDivElement;
    toolbar: Toolbar;
    /**
     * 动态重置工具栏配置
     * @public
     * @param {'toolbar'|'toolbarRight'|'sidebar'|'bubble'|'float'} [type] 修改工具栏的类型
     * @param {Array} [toolbar] 要重置的对应工具栏配置
     * @returns {Boolean}
     */
    public resetToolbar(type?: 'toolbar' | 'toolbarRight' | 'sidebar' | 'bubble' | 'float', toolbar?: any[]): boolean;
    /**
     * @private
     * @returns {Toolbar}
     */
    private createToolbarRight;
    toolbarRight: ToolbarRight;
    /**
     * @private
     * @returns
     */
    private createSidebar;
    sidebarDom: HTMLDivElement;
    sidebar: Sidebar;
    /**
     * @private
     * @returns
     */
    private createFloatMenu;
    toolbarFloatContainer: HTMLDivElement;
    floatMenu: FloatMenu;
    /**
     * @private
     * @returns
     */
    private createBubble;
    toolbarBubbleContainer: HTMLDivElement;
    bubble: Bubble;
    /**
     * @private
     * @returns {import('@/Editor').default}
     */
    private createEditor;
    editor: Editor;
    /**
     * @private
     * @returns {import('@/Previewer').default}
     */
    private createPreviewer;
    previewer: Previewer;
    /**
     * @private
     * @param {import('codemirror').Editor} codemirror
     */
    private initText;
    /**
     * @private
     * @param {Event} _evt
     * @param {import('codemirror').Editor} codemirror
     */
    private editText;
    timer: NodeJS.Timeout;
    /**
     * @private
     * @param {any} cb
     */
    private onChange;
    /**
     * @private
     * @param {KeyboardEvent} evt
     */
    private fireShortcutKey;
    /**
     * 导出预览区域内容
     * @public
     * @param {'pdf' | 'img' | 'markdown' | 'html'} [type='pdf']
     * 'pdf'：导出成pdf文件; 'img'：导出成png图片; 'markdown'：导出成markdown文件; 'html'：导出成html文件;
     * @param {string} [fileName] 导出文件名(默认为当前第一行内容|'cherry-export')
     */
    public export(type?: 'pdf' | 'img' | 'markdown' | 'html', fileName?: string): void;
    /**
     * 修改主题
     * @param {string} theme option.theme里的className
     */
    setTheme(theme?: string): void;
    /**
     * 修改书写风格
     * @param {string} writingStyle normal 普通 | typewriter 打字机 | focus 专注
     */
    setWritingStyle(writingStyle: string): void;
}
export type CherryOptions = import('../../types/cherry').CherryOptions;
import { CherryStatic } from "./CherryStatic";
import Event from "./Event";
import Toc from "./toolbars/Toc";
import Editor from "./Editor";
import Previewer from "./Previewer";
import Toolbar from "./toolbars/Toolbar";
import ToolbarRight from "./toolbars/ToolbarRight";
import Sidebar from "./toolbars/Sidebar";
import FloatMenu from "./toolbars/FloatMenu";
import Bubble from "./toolbars/Bubble";
