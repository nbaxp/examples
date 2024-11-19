export function createSerie(value = {}) {
  const defaultValue = {
    smooth: true,
    type: 'bar',
    data: [],
  };
  Object.assign(value, defaultValue);
  return value;
}

export default function (options = {}) {
  const defaultOptions = {
    title: {
      left: 'center',
      text: '',
    },
    legend: {
      show: true,
      data: [],
    },
    xAxis: {
      type: 'category',
      data: [],
      axisLabel: {
        rotate: 45,
      },
      splitLine: {
        show: true,
        interval: 1,
      },
      splitArea: {
        show: true,
      },
    },
    yAxis: {
      type: 'value',
      axisLine: {
        show: true,
      },
    },
    series: [createSerie()],
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        lineStyle: {
          color: null,
        },
      },
    },
  };
  Object.assign(options, defaultOptions);
  return options;
}
