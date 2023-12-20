FROM python:3

WORKDIR /usr/src/app

COPY Measure_Stats.py /usr/src/app

CMD [ "python", "./Measure_Stats.py" ]