/**
 * 插入行内公式
 * @see https://github.com/QianJianTech/LaTeXLive/blob/master/README.md
 */
export default class Formula extends MenuBase {
    constructor($cherry: any);
    subBubbleFormulaMenu: BubbleFormula;
    catchOnce: string;
}
import MenuBase from "@/toolbars/MenuBase";
import BubbleFormula from "../BubbleFormula";
