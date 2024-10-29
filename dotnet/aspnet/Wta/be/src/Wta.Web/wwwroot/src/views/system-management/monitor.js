import Chart from '@/components/chart/index.js';
import html from 'utils';
import { onActivated, onDeactivated, ref } from 'vue';

export default {
  components: { Chart },
  template: html`
    <div class="container xl">
      <el-row :gutter="20" class="mb-5">
        <el-col :span="12">
          <el-card>
            <chart :option="cpuModel" height="300px" />
          </el-card>
        </el-col>
        <el-col :span="12">
          <el-card>
            <chart :option="memoryModel" height="300px" />
          </el-card>
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="12">
          <el-card>
            <chart :option="networkModel" height="300px" />
          </el-card>
        </el-col>
        <el-col :span="12">
          <el-card>
            <chart :option="diskModel" height="300px" />
          </el-card>
        </el-col>
      </el-row>
    </div>
  `,
  setup() {
    const model = ref(null);
    const timer = ref(null);
    const seconds = 1;
    onActivated(() => {
      timer.value = setInterval(load, seconds * 1000);
    });
    onDeactivated(() => clearInterval(timer.value));
    //reduce
    const sum = (p, c) => p + c.value;
    //cpu
    const cpuModel = ref({
      title: {
        text: '',
      },
      xAxis: {
        type: 'category',
        data: Object.keys(Array(30).fill()),
      },
      yAxis: {
        type: 'value',
        min: 0,
        max: 100,
      },
      series: [
        {
          data: [],
          type: 'line',
          smooth: true,
          areaStyle: {},
          showSymbol: false,
        },
      ],
    });
    let cpuUsage1 = null;
    const updateCpu = () => {
      const cpuUsage2 = {
        idle: model.value.node_cpu_seconds_total.filter((o) => o.mode === 'idle').reduce(sum, 0),
        total: model.value.node_cpu_seconds_total.reduce(sum, 0),
      };
      if (!cpuUsage1) {
        cpuUsage1 = cpuUsage2;
      } else {
        const cpuUsage = cpuUsage2.idle / cpuUsage2.total;
        if (cpuModel.value.series[0].data.length > 60) {
          cpuModel.value.series[0].data.shift();
        }
        cpuModel.value.series[0].data.push(cpuUsage);
        cpuModel.value.title.text = 'CPU';
      }
    };

    //memory
    const memoryModel = ref({
      title: {
        text: '',
      },
      legend: {
        data: ['节点', '进程'],
      },
      xAxis: {
        type: 'category',
        data: Object.keys(Array(30).fill()),
      },
      yAxis: {
        type: 'value',
        min: 0,
        max: 100,
      },
      series: [
        {
          name: '节点',
          data: [],
          type: 'line',
          smooth: true,
          showSymbol: false,
        },
        {
          name: '进程',
          data: [],
          type: 'line',
          smooth: true,
          showSymbol: false,
        },
      ],
    });

    const updateMemory = () => {
      if (memoryModel.value.series[0].data.length > 60) {
        memoryModel.value.series[0].data.shift();
      }
      if (memoryModel.value.series[1].data.length > 60) {
        memoryModel.value.series[1].data.shift();
      }
      const totalMemory = model.value.node_memory_MemTotal_bytes.reduce(sum, 0);
      const memoryUsage = ((1 - model.value.node_memory_MemAvailable_bytes.reduce(sum, 0) / totalMemory) * 100).toFixed(
        2,
      );
      const processMemoryUsage = (model.value.process_private_memory_bytes.reduce(sum, 0) / totalMemory).toFixed(2);
      memoryModel.value.series[0].data.push(memoryUsage);
      memoryModel.value.series[1].data.push(processMemoryUsage);
      memoryModel.value.title.text = '内存';
    };

    //network
    const networkModel = ref({
      title: {
        text: '',
      },
      legend: {
        data: ['接收', '发送'],
      },
      xAxis: {
        type: 'category',
        data: Object.keys(Array(30).fill()),
      },
      yAxis: {
        type: 'value',
      },
      series: [
        {
          name: '接收',
          data: [],
          type: 'line',
          smooth: true,
          showSymbol: false,
        },
        {
          name: '发送',
          data: [],
          type: 'line',
          smooth: true,
          showSymbol: false,
        },
      ],
    });
    let networkReceive1 = null;
    let networkTransmit1 = null;
    const updateNetwork = () => {
      const networkReceive2 = model.value.node_network_receive_bytes_total.reduce(sum, 0);
      const networkTransmit2 = model.value.node_network_transmit_bytes_total.reduce(sum, 0);
      const networkReceive = Number.parseInt((networkReceive2 - networkReceive1) / 1024 / seconds);
      const networkTransmit = Number.parseInt((networkTransmit2 - networkTransmit1) / 1024 / seconds);
      if (!networkReceive1) {
        networkReceive1 = networkReceive2;
      } else {
        if (networkModel.value.series[0].data.length > 60) {
          networkModel.value.series[0].data.shift();
        }
        networkModel.value.series[0].data.push(networkReceive);
      }

      if (!networkTransmit1) {
        networkTransmit1 = networkTransmit2;
      } else {
        if (networkModel.value.series[1].data.length > 60) {
          networkModel.value.series[1].data.shift();
        }
        networkModel.value.series[1].data.push(networkTransmit);
      }
      networkModel.value.title.text = '网络';
    };

    //dist
    const diskModel = ref({
      title: {
        text: '',
      },
      legend: {
        data: [],
      },
      xAxis: {
        type: 'category',
        data: Object.keys(Array(30).fill()),
      },
      yAxis: {
        type: 'value',
        smooth: true,
        showSymbol: false,
      },
      series: [],
    });

    const updateDisk = () => {
      for (let i = 0; i < model.value.node_filesystem_avail_bytes.length; i++) {
        const item = model.value.node_filesystem_avail_bytes[i];
        if (!diskModel.value.series[i]) {
          diskModel.value.series[i] = {
            name: item.mountpoint,
            data: [],
            type: 'line',
            smooth: true,
          };
          diskModel.value.legend.data[i] = item.mountpoint;
        }
        if (diskModel.value.series[i].data.length > 60) {
          diskModel.value.series[i].data.shift();
        }
        diskModel.value.series[i].data.push(Number.parseInt(item.value / 1024 / 1024 / 1024));
      }
      diskModel.value.title.text = '硬盘';
    };

    const load = async () => {
      const response = await fetch('/api/metrics');
      const result = await response.text();
      const lines = result.split('\n').filter((o) => o);
      const types = new Map(
        lines
          .filter((o) => o.startsWith('# TYPE'))
          .map((o) => {
            const [, , key, value] = o.split(' ');
            return [key, value];
          }),
      );
      model.value = Object.groupBy(
        lines
          .filter((o) => !o.startsWith('#'))
          .map((o) => {
            const [, name, , label, value] = /^([^{}]+)(\{(.+)\})? (.+)$/.exec(o);
            const result = {
              name,
              value: Number(value),
            };
            result.type = types.get(name);
            if (label) {
              const values = label
                .split(',')
                .map((o) => o.split('='))
                .map((o) => [o[0], o[1].slice(1, -1)]);
              const labels = Object.fromEntries(values);
              Object.assign(result, labels);
            }
            return result;
          }),
        (o) => o.name,
      );

      updateCpu();
      updateMemory();
      updateNetwork();
      updateDisk();
    };
    return {
      model,
      cpuModel,
      memoryModel,
      networkModel,
      diskModel,
      // dayjs,
      // bytesFormat,
      // persentFormat,
    };
  },
};
