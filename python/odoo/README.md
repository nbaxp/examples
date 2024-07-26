# odoo

## 环境设置

1. 安装 Python 3.10 + 并确保 Path 环境变量包含程序路径：<https://www.python.org/downloads/windows/>

1. 检查安装：`python --version && pip --version`
1. 安装 PostgreSQL 12.0 + :<https://www.enterprisedb.com/downloads/postgres-postgresql-downloads>
1. 安装 Build Tools for Visual Studio 2022：<https://visualstudio.microsoft.com/downloads/>
1. 获取 odoo 源码到 CommunityPath 目录，安装依赖：

   ```sh
   set LC_ALL="en_US.utf8"
   pip install ipdb
   pip install setuptools wheel
   pip install -r requirements.txt
   ```

1. 从右到左的语言需要下载并安装 node.js，全局安装 rtlcss：`npm install -g rtlcss`，并设置系统环境变量 PATH 包含 rtlcss.cmd 所在目录
1. 安装 wkhtmlpdf 并设置系统环境变量 Path：<https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox-0.12.6-1.msvc2015-win64.exe>

1. 启动项目：`python3 odoo-bin -c config/odoo.conf`
1. 调试

```sh
pip install debugpy
```

## 架构

1. 前端： es 6 + 私有前端框架 owl:<https://odoo.github.io/owl/>
1. 后端： python 3.10 + 私有 orm 框架: <https://www.odoo.com/documentation/17.0/developer/reference/backend/orm.html>
