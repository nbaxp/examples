import html  from "utils";
import { reactive, ref } from "vue";
import { dayjs } from "element-plus";

export default {
  template: html`<div class="app-table el-table" ref="tableRef">
    <div class="row header">
      <template v-for="(item,key) in columns.properties">
        <div class="cell el-table__cell selection" v-if="item.input==='selection'">
          <div class="cell" style="text-align:center;">
            <input class="el-checkbox__input" type="checkbox" v-model="checkAll" :indeterminate="checkAllIndeterminate" @click="checkAllClick($event)" />
          </div>
        </div>
        <template v-else-if="item.type==='object'">
          <template v-for="(item2,key2) in item.properties">
            <div class="cell el-table__cell" :class="key+'.'+key2" v-if="!item2.hidden">
              <div class="cell">{{item2.title}}</div>
            </div>
          </template>
        </template>
        <div class="cell el-table__cell" :class="key" v-else-if="!item.hidden">
          <div class="cell">{{item.title}} <template v-if="data.length&&item.reduce">:{{Math.round(100*data.map(o=>o[key]).reduce(item.reduce))/100}}</template></div>
        </div>
      </template>
    </div>
    <div v-for="(row,index) in data" class="row data">
      <template v-for="(item,key) in columns.properties">
        <div class="cell el-table__cell selection" v-if="item.input==='selection'">
          <div class="cell" style="text-align:center;" v-if="item.input==='selection'">
            <input class="el-checkbox__input row" type="checkbox" :value="index" @click="checkClick" />
          </div>
        </div>
        <template v-else-if="item.type==='object'">
          <template v-for="(item2,key2) in item.properties">
            <div class="cell el-table__cell" :class="key+'.'+key2" v-if="!item2.hidden">
              <div class="cell" v-if="row[key]&&!item.hidden">{{row[key][key2]}}</div>
            </div>
          </template>
        </template>
        <div class="cell el-table__cell" :class="key" v-else-if="!item.hidden">
          <div class="cell">
            <template v-if="item.input==='datetime'">{{dayjs(row[key]).format('YYYY-MM-DD HH:mm:ss')}} {{}}</template>
            <template v-else>{{row[key]}}</template>
          </div>
        </div>
      </template>
    </div>
  </div>`,
  styles: html`<style>
    .app-table {
      display: table;
      content-visibility: visible;
      border-top: var(--el-table-border);
      border-left: var(--el-table-border);
    }
    .app-table .row {
      display: table-row;
      background-color: var(--el-table-tr-bg-color);
    }
    .app-table .cell.el-table__cell {
      display: table-cell;
      padding: 8px 0;
      border-right: var(--el-table-border);
      border-bottom: var(--el-table-border);
    }
  </style>`,
  props: ["modelValue", "columns", "data"],
  setup(props) {
    const model = reactive(props.data);
    const tableRef = ref(null);
    const getSelection = () => {
      return Array.from(tableRef.value.querySelectorAll("input.row:checked")).map((o) => parseInt(o.value));
    };
    const clearSelection = () => {
      Array.from(tableRef.value.querySelectorAll("input:checked")).forEach((o) => (o.checked = false));
    };
    const checkAll = ref(false);
    const checkAllIndeterminate = ref(false);
    const checkAllClick = (e) => {
      checkAll.value = !checkAll.value;
      if (checkAll.value) {
        Array.from(tableRef.value.querySelectorAll("input.row:not(:checked)")).forEach((o) => (o.checked = true));
      } else {
        Array.from(tableRef.value.querySelectorAll("input.row:checked")).forEach((o) => (o.checked = false));
      }
      const checkdCount = Array.from(tableRef.value.querySelectorAll("input.row:checked")).length;
      checkAllIndeterminate.value = checkdCount > 0 && checkdCount < props.data.length;
    };
    const checkClick = () => {
      const checkdCount = Array.from(tableRef.value.querySelectorAll("input.row:checked")).length;
      checkAllIndeterminate.value = checkdCount > 0 && checkdCount < props.data.length;
      checkAll.value = checkdCount == props.data.length;
    };
    return {
      model,
      tableRef,
      checkAll,
      checkClick,
      checkAllIndeterminate,
      checkAllClick,
      getSelection,
      clearSelection,
      dayjs,
    };
  },
};
