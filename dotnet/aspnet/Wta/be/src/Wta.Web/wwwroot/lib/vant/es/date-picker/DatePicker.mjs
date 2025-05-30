import { createVNode as _createVNode, mergeProps as _mergeProps } from "vue";
import { ref, watch, computed, defineComponent } from "vue";
import { pick, extend, isDate, isSameValue, createNamespace } from "../utils/index.mjs";
import { genOptions, sharedProps, getMonthEndDay, pickerInheritKeys, formatValueRange } from "./utils.mjs";
import { useExpose } from "../composables/use-expose.mjs";
import { Picker } from "../picker/index.mjs";
const currentYear = (/* @__PURE__ */ new Date()).getFullYear();
const [name] = createNamespace("date-picker");
const datePickerProps = extend({}, sharedProps, {
  columnsType: {
    type: Array,
    default: () => ["year", "month", "day"]
  },
  minDate: {
    type: Date,
    default: () => new Date(currentYear - 10, 0, 1),
    validator: isDate
  },
  maxDate: {
    type: Date,
    default: () => new Date(currentYear + 10, 11, 31),
    validator: isDate
  }
});
var stdin_default = defineComponent({
  name,
  props: datePickerProps,
  emits: ["confirm", "cancel", "change", "update:modelValue"],
  setup(props, {
    emit,
    slots
  }) {
    const currentValues = ref(props.modelValue);
    const updatedByExternalSources = ref(false);
    const pickerRef = ref();
    const genYearOptions = () => {
      const minYear = props.minDate.getFullYear();
      const maxYear = props.maxDate.getFullYear();
      return genOptions(minYear, maxYear, "year", props.formatter, props.filter);
    };
    const isMinYear = (year) => year === props.minDate.getFullYear();
    const isMaxYear = (year) => year === props.maxDate.getFullYear();
    const isMinMonth = (month) => month === props.minDate.getMonth() + 1;
    const isMaxMonth = (month) => month === props.maxDate.getMonth() + 1;
    const getValue = (type) => {
      const {
        minDate,
        columnsType
      } = props;
      const index = columnsType.indexOf(type);
      const value = updatedByExternalSources.value ? props.modelValue[index] : currentValues.value[index];
      if (value) {
        return +value;
      }
      switch (type) {
        case "year":
          return minDate.getFullYear();
        case "month":
          return minDate.getMonth() + 1;
        case "day":
          return minDate.getDate();
      }
    };
    const genMonthOptions = () => {
      const year = getValue("year");
      const minMonth = isMinYear(year) ? props.minDate.getMonth() + 1 : 1;
      const maxMonth = isMaxYear(year) ? props.maxDate.getMonth() + 1 : 12;
      return genOptions(minMonth, maxMonth, "month", props.formatter, props.filter);
    };
    const genDayOptions = () => {
      const year = getValue("year");
      const month = getValue("month");
      const minDate = isMinYear(year) && isMinMonth(month) ? props.minDate.getDate() : 1;
      const maxDate = isMaxYear(year) && isMaxMonth(month) ? props.maxDate.getDate() : getMonthEndDay(year, month);
      return genOptions(minDate, maxDate, "day", props.formatter, props.filter);
    };
    const confirm = () => {
      var _a;
      return (_a = pickerRef.value) == null ? void 0 : _a.confirm();
    };
    const getSelectedDate = () => currentValues.value;
    const columns = computed(() => props.columnsType.map((type) => {
      switch (type) {
        case "year":
          return genYearOptions();
        case "month":
          return genMonthOptions();
        case "day":
          return genDayOptions();
        default:
          if (process.env.NODE_ENV !== "production") {
            throw new Error(`[Vant] DatePicker: unsupported columns type: ${type}`);
          }
          return [];
      }
    }));
    watch(currentValues, (newValues) => {
      if (!isSameValue(newValues, props.modelValue)) {
        emit("update:modelValue", newValues);
      }
    });
    watch(() => props.modelValue, (newValues, oldValues) => {
      updatedByExternalSources.value = isSameValue(oldValues, currentValues.value);
      newValues = formatValueRange(newValues, columns.value);
      if (!isSameValue(newValues, currentValues.value)) {
        currentValues.value = newValues;
      }
      updatedByExternalSources.value = false;
    }, {
      immediate: true
    });
    const onChange = (...args) => emit("change", ...args);
    const onCancel = (...args) => emit("cancel", ...args);
    const onConfirm = (...args) => emit("confirm", ...args);
    useExpose({
      confirm,
      getSelectedDate
    });
    return () => _createVNode(Picker, _mergeProps({
      "ref": pickerRef,
      "modelValue": currentValues.value,
      "onUpdate:modelValue": ($event) => currentValues.value = $event,
      "columns": columns.value,
      "onChange": onChange,
      "onCancel": onCancel,
      "onConfirm": onConfirm
    }, pick(props, pickerInheritKeys)), slots);
  }
});
export {
  datePickerProps,
  stdin_default as default
};
