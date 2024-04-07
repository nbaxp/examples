# 注释
# 文档：https://docs.docker.com/reference/dockerfile/

# FROM 基础镜像
FROM nginx:1.24.0

# LABEL 元数据
LABEL org.opencontainers.image.authors="demo@demo.com"

# 构建参数，运行时不存在
ARG DEMO_VERSION=1.0.0

# 环境变量
ENV DEMO_VERSION=${DEMO_VERSION}

# RUN 命令
RUN groupadd -r demo && useradd -r -g demo demo

# USER 指定用户
# USER demo:demo

# WORKDIR 默认值
WORKDIR /

# COPY 复制
# 不推荐使用 ADD
COPY wwwroot/ /usr/share/nginx/html

# SHELL 默认值
SHELL ["/bin/bash", "-c"]

# EXPOSE 导出端口
EXPOSE 80
EXPOSE 443

# VOLUME 自动挂载
VOLUME ["/app/data"]

# CMD 启动命令
# CMD shell 格式
# CMD nginx -g daemon off;
# 等效于 CMD sh -c "nginx -g daemon off;"
# CMD exec 格式
CMD ["-g","daemon off;"]

# ENTRYPOINT 启用该指令后，CMD 指令将作为参数传入
ENTRYPOINT ["nginx"]

# HEALTHCHECK 健康检查
HEALTHCHECK --interval=5s --timeout=5s \
    CMD curl -f http://localhost/ || exit 1

# ONBUILD 以此作为基础镜像时自动执行的命令