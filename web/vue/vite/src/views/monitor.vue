<template>
  <el-row :gutter="20" style="margin-bottom: 20px">
    <el-col :span="24">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <span> {{ t('host') }}</span>
          </div>
        </template>
        <el-descriptions border>
          <el-descriptions-item :label="t('serverTime')"
            >{{ dayjs(model.serverTime).format('YYYY-MM-DD HH:mm:ss') }}
          </el-descriptions-item>
          <el-descriptions-item :label="t('cpuCount')">{{ model.cpuCount }} </el-descriptions-item>
          <el-descriptions-item :label="t('memoryTotal')">{{ bytesFormat(model.memoryTotal) }} </el-descriptions-item>
          <el-descriptions-item :label="t('osArchitecture')">{{ model.osArchitecture }} </el-descriptions-item>
          <el-descriptions-item :label="t('osDescription')">{{ model.osDescription }} </el-descriptions-item>
          <el-descriptions-item :label="t('hostName')">{{ model.hostName }} </el-descriptions-item>
          <el-descriptions-item :label="t('runtimeIdentifier')">{{ model.runtimeIdentifier }} </el-descriptions-item>
          <el-descriptions-item :label="t('userName')">{{ model.userName }} </el-descriptions-item>
          <el-descriptions-item :label="t('processCount')">{{ model.processCount }} </el-descriptions-item>
          <el-descriptions-item :label="t('hostAddresses')">{{ model.hostAddresses }} </el-descriptions-item>
        </el-descriptions>
      </el-card>
    </el-col>
  </el-row>
  <el-row :gutter="20" style="margin-bottom: 20px">
    <el-col :span="12">
      <el-card class="box-card">
        <chart :option="cpuModel" height="300px" />
      </el-card>
    </el-col>
    <el-col :span="12">
      <el-card class="box-card">
        <chart :option="memoryModel" height="300px" />
      </el-card>
    </el-col>
  </el-row>
  <el-row :gutter="20" style="margin-bottom: 20px">
    <el-col :span="24">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <span> {{ t('process') }}</span>
          </div>
        </template>
        <el-descriptions border :column="3">
          <el-descriptions-item :label="t('processArchitecture')">
            {{ model.processArchitecture }}
          </el-descriptions-item>
          <el-descriptions-item :label="t('processId')">{{ model.processId }} </el-descriptions-item>
          <el-descriptions-item :label="t('processName')">{{ model.processName }} </el-descriptions-item>
          <el-descriptions-item :label="t('processArguments')">{{ model.processArguments }} </el-descriptions-item>
          <el-descriptions-item :label="t('processHandleCount')">{{ model.processHandleCount }} </el-descriptions-item>
          <el-descriptions-item :label="t('processFileName')">{{ model.processFileName }} </el-descriptions-item>
          <el-descriptions-item :label="t('driveName')">{{ model.driveName }} </el-descriptions-item>
          <el-descriptions-item :label="t('drivieTotalSize')"
            >{{ bytesFormat(model.drivieTotalSize) }}
          </el-descriptions-item>
          <el-descriptions-item :label="t('driveAvailableFreeSpace')">
            {{ bytesFormat(model.driveAvailableFreeSpace) }}
          </el-descriptions-item>
        </el-descriptions>
      </el-card>
    </el-col>
  </el-row>
  <el-row :gutter="20" style="margin-bottom: 20px">
    <el-col :span="24">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <span> {{ t('framework') }}</span>
          </div>
        </template>
        <el-descriptions border :column="3">
          <el-descriptions-item :label="t('framework')">{{ model.framework }} </el-descriptions-item>
          <el-descriptions-item :label="t('exceptionCount')">{{ model.exceptionCount }} </el-descriptions-item>
          <el-descriptions-item :label="t('totalRequests')">{{ model.totalRequests }} </el-descriptions-item>
          <el-descriptions-item :label="t('bytesReceived')">
            {{ bytesFormat(model.bytesReceived) }}
          </el-descriptions-item>
          <el-descriptions-item :label="t('bytesSent')">{{ bytesFormat(model.bytesSent) }} </el-descriptions-item>
        </el-descriptions>
      </el-card>
    </el-col>
  </el-row>
</template>

<script setup>
  import { reactive, onMounted } from 'vue';
  import { useI18n } from 'vue-i18n';
  import Chart from '@/components/chart/index.vue';
  import { persentFormat, bytesFormat } from '@/utils/index.js';
  import { dayjs } from 'element-plus';

  const hasEventSource = !!window.EventSource;
  const { t } = useI18n();

  const model = reactive({});

  const cpuModel = reactive({
    title: {
      text: t('cpuUsage'),
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
      },
    ],
  });

  const memoryModel = reactive({
    title: {
      text: t('memoryUsage'),
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
      },
    ],
  });

  const update = (data) => {
    Object.assign(model, data);
    // cpu
    if (cpuModel.series[0].data.length > 60) {
      cpuModel.series[0].data.shift();
    }
    cpuModel.title.text = `${t('cpuUsage')}:${persentFormat(model.cpuUsage / 100)}`;
    cpuModel.series[0].data.push(data.cpuUsage);
    // memory
    if (memoryModel.series[0].data.length > 60) {
      memoryModel.series[0].data.shift();
    }
    memoryModel.title.text = `${t('memoryUsage')}:${persentFormat(model.memoryUsage / 100)}`;
    memoryModel.series[0].data.push(data.memoryUsage);
  };

  const connect = () => {
    const es = new EventSource('/api/monitor/index');
    es.onmessage = (event) => {
      update(JSON.parse(event.data));
    };
    es.onerror = (e) => {
      es.close();
      console.log(e);
      setTimeout(connect, 5 * 1000);
    };
  };

  onMounted(() => {
    if (hasEventSource) {
      try {
        connect();
      } catch (e) {
        console.log(e);
        setTimeout(connect, 5 * 1000);
      }
    }
  });
</script>
