var lineColor = ['#f00707', '#2b24ff', '#089938',
'#dc9b3e', '#ffff9f', '#4d8ea6'];
var lineTick = 0;
var func_Line = function basic_time(container, data1, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      d1 = [],
      start = new Date("2009/01/01 01:00").getTime(),
      options,
      graph,
      i, x, o;



    options = {
        colors: lineColor,
        xaxis: {
            showLabels: showxlabel,
            mode: 'time',
            labelsAngle: 0,
        },
        yaxis: {
            showLabels: showylabel,
            min: 0,
            max: maxYvalue,
            //tickDecimals:10,
            //noTicks: lineTick
        }, y2axis: {
            showLabels: showy2label,
            min: 0,
            max: maxYvalue,
            //tickDecimals:10,
            //noTicks: lineTick
        }, legend: {
            show: showlegend
        }, series: {
            lines: {
                show: true
            },
            points: {
                show: false
            }
        }, grid: {
            tickColor: '#c4c4c4',
            backgroundColor: {
                colors: ['#e0deff', '#e1f4f7'],
                start: 'top',
                end: 'bottom'
            },//'#e0deff', 
            verticalLines: vertical,
            horizontalLines: horizon,
        }, lines: {
            lineWidth: 0.5,
        },
        selection: {
            mode: 'x'
        },
        HtmlText: false,
    };

    // Draw graph with default options, overwriting with passed options
    function drawGraph(opts) {

        // Clone the options, so the 'options' variable always keeps intact.
        o = Flotr._.extend(Flotr._.clone(options), opts || {});

        // Return a new graph.
        return Flotr.draw(
          container,
          data1,
          o
        );
    }

    graph = drawGraph();

    Flotr.EventAdapter.observe(container, 'flotr:select', function (area) {
        // Draw selected area
        graph = drawGraph({
            xaxis: { min: area.x1, max: area.x2, mode: 'time', labelsAngle: 0, showLabels: showxlabel },
            yaxis: { min: area.y1, max: area.y2, showLabels: showylabel },
            lines: {
                lineWidth: 0.5,
            }
        });
    });

    // When graph is clicked, draw the graph with default area.
    Flotr.EventAdapter.observe(container, 'flotr:click', function () { graph = drawGraph(); });
};

var func_DualYLine = function basic_time(container, data1, data2, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label, ytitle, y2title) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      d1 = [],
      start = new Date("2009/01/01 01:00").getTime(),
      options,
      graph,
      i, x, o;



    options = {
        colors: lineColor,
        xaxis: {
            showLabels: showxlabel,
            mode: 'time',
            labelsAngle: 0,
        },
        yaxis: {
            showLabels: showylabel,
            min: 0,
            max: maxYvalue,
            //tickDecimals:10,
            //noTicks: lineTick
            title: ytitle
        }, y2axis: {
            showLabels: showy2label,
            min: 0,
            max: maxYvalue,
            title: y2title
            //tickDecimals:10,
            //noTicks: lineTick
        }, legend: {
            show: showlegend
        }, grid: {
            tickColor: '#c4c4c4',
            backgroundColor: {
                colors: ['#e0deff', '#e1f4f7'],
                start: 'top',
                end: 'bottom'
            },//'#e0deff', 
            verticalLines: vertical,
            horizontalLines: horizon,
        }, lines: {
            lineWidth: 0.5,
        },
        selection: {
            mode: 'x'
        },
        HtmlText: false,

    };

    // Draw graph with default options, overwriting with passed options
    function drawGraph(opts) {

        // Clone the options, so the 'options' variable always keeps intact.
        o = Flotr._.extend(Flotr._.clone(options), opts || {});

        // Return a new graph.
        return Flotr.draw(
          container,
          [data1, data2],
          o
        );
    }

    graph = drawGraph();

    Flotr.EventAdapter.observe(container, 'flotr:select', function (area) {
        // Draw selected area
        graph = drawGraph({
            xaxis: { min: area.x1, max: area.x2, mode: 'time', labelsAngle: 0, showLabels: showxlabel },
            yaxis: { min: area.y1, max: area.y2, showLabels: showylabel },
            //y2axis: { min: area.y1, max: area.y2, showLabels: showy2label },
            lines: {
                lineWidth: 0.5,
            }
        });
    });

    // When graph is clicked, draw the graph with default area.
    Flotr.EventAdapter.observe(container, 'flotr:click', function () { graph = drawGraph(); });
};
var func_Line3 = function basic_time(container, data1, data2, data3, maxYvalue, widthPx, heightPx, horizon, vertical) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      d1 = [],
      start = new Date("2009/01/01 01:00").getTime(),
      options,
      graph,
      i, x, o;



    options = {
        xaxis: {
            showLabels: false,
            mode: 'time',
            labelsAngle: 45,
            noTicks: lineTick
        },
        yaxis: {
            showLabels: false,
            max: maxYvalue,
            noTicks: 10
        },
        selection: {
            mode: 'x'
        }, lines: {
            lineWidth: 0.5,
        },
        grid: {

            tickColor: '#c4c4c4',
            backgroundColor: {
                colors: ['#e0deff', '#e1f4f7'],
                start: 'top',
                end: 'bottom'
            },//'#e0deff', 
            verticalLines: vertical,
            horizontalLines: horizon,
        },
        colors: lineColor,
        HtmlText: false,

    };

    // Draw graph with default options, overwriting with passed options
    function drawGraph(opts) {

        // Clone the options, so the 'options' variable always keeps intact.
        o = Flotr._.extend(Flotr._.clone(options), opts || {});

        // Return a new graph.
        return Flotr.draw(
          container,
          [data1, data2, data3],
          o
        );
    }

    graph = drawGraph();

    Flotr.EventAdapter.observe(container, 'flotr:select', function (area) {
        // Draw selected area
        graph = drawGraph({
            xaxis: { min: area.x1, max: area.x2, mode: 'time', labelsAngle: 45, showLabels: false },
            yaxis: { min: area.y1, max: area.y2, showLabels: false }, lines: {
                lineWidth: 0.5,
            }
        });
    });

    // When graph is clicked, draw the graph with default area.
    Flotr.EventAdapter.observe(container, 'flotr:click', function () { graph = drawGraph(); });
};
var func_Bar = function basic_timeline(container, arr, nameIs, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      //d1 = [[0,4, 100]],
      //d2 = [[0, 3, 4]],
      data = [],
      timeline = { show: true, barWidth: 1, fill: false, fillColor: { colors: ['#9bcfa7', '#067302'], start: 'left', end: 'right' }, horizontal: true, },
       markers = [],
      labels = nameIs,
      i, graph, point;
    // Timeline
    Flotr._.each(arr, function (d) {
        data.push({
            data: d,
            timeline: Flotr._.clone(timeline)
        });
    });

    // Markers
    Flotr._.each(arr, function (d) {
        point = d[0];
        markers.push([point[0], point[1]]);
    });
    data.push({
        data: markers,
        markers: {
            show: true,
            position: 'rm',
            fontSize: 11,
            labelFormatter: function (o) { return labels[o.index]; }
        }
    });

    // Draw Graph
    graph = Flotr.draw(container, data, {
        xaxis: {
            showLabels: false,
            max: maxYvalue,
            noTicks: 10
        },
        yaxis: {
            showLabels: false
        },
        grid: {

            tickColor: '#c4c4c4',
            backgroundColor: {
                colors: ['#e1f4f7', '#e0deff'],
                start: 'left',
                end: 'right'
            },//'#e0deff', 
            verticalLines: vertical,
            horizontalLines: horizon,
        }
    });
};
var func_Cols = function color_gradients(container, arr, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      d1 = [arr],

        options,
        graph,
        i;



    options = {
        selection: { mode: 'x', fps: 30 },
        bars: {
            show: true,
            barWidth: 0.8,
            lineWidth: 1,
            //fillColor: {
            //    colors: ['#089938', '#089938'],
            //    start: 'top',
            //    end: 'bottom'
            //},
            fillOpacity: 0.8
        }, yaxis: {
            showLabels: showylabel, min: 0,
            max: maxYvalue,
            noTicks: 10
        },
        xaxis: {
            mode: 'time',
            showLabels: showxlabel, min: 0
        },
        legend: {
            show: showlegend
        },
        grid: {
            tickColor: '#c4c4c4',
            backgroundColor: '#d7deed',
            verticalLines: vertical,
            horizontalLines: horizon,
        },
    };

    // Draw graph with default options, overwriting with passed options
    function drawGraph(opts) {

        // Clone the options, so the 'options' variable always keeps intact.
        var o = Flotr._.extend(Flotr._.clone(options), opts || {});

        // Return a new graph.
        return Flotr.draw(
          container,
          d1,
          o
        );
    }

    // Actually draw the graph.
    graph = drawGraph();

    // Hook into the 'flotr:select' event.
    Flotr.EventAdapter.observe(container, 'flotr:select', function (area) {

        // Draw graph with new area
        graph = drawGraph({
            selection: { mode: 'x', fps: 30 },
            bars: {
                show: true,
                barWidth: 0.8,
                lineWidth: 1,
                //fillColor: {
                //    colors: ['#089938', '#089938'],
                //    start: 'top',
                //    end: 'bottom'
                //},
                fillOpacity: 0.8
            }, yaxis: {
                showLabels: showylabel, min: 0,
                max: maxYvalue,
                noTicks: 10
            },
            xaxis: {
                showLabels: showxlabel
            },
            legend: {
                show: showlegend
            },
            grid: {
                tickColor: '#c4c4c4',
                backgroundColor: '#d7deed',
                verticalLines: vertical,
                horizontalLines: horizon,
            },
            xaxis: { min: area.x1, max: area.x2, showLabels: false },
            yaxis: {
                min: area.y1, max: area.y2, showLabels: false, min: 0, max: maxYvalue,
                noTicks: 10
            }
        });
    });

    // When graph is clicked, draw the graph with default area.
    Flotr.EventAdapter.observe(container, 'flotr:click', function () { drawGraph(); });
}
var func_Cols_Horizon = function color_gradients(container, arr, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      d1 = arr,

        options,
        graph,
        i;



    options = {
        selection: { mode: 'x', fps: 30 },
        bars: {
            show: true,
            barWidth: 0.3,
            //lineWidth: 1,
            //fillColor: {
            //    colors: ['#089938', '#089938'],
            //    start: 'top',
            //    end: 'bottom'
            //},
            horizontal: true,
            fillOpacity: 0.8
        }, yaxis: {
            showLabels: showylabel, min: 0,
            max: maxYvalue,
            noTicks: 10
        },
        xaxis: {
            showLabels: showxlabel, min: 0
        },
        legend: {
            show: showlegend
        },
        grid: {
            tickColor: '#c4c4c4',
            backgroundColor: {
                colors: [[0, '#fff'], [1, '#e3e3e3']],
                start: 'top',
                end: 'bottom'
            },
            verticalLines: vertical,
            horizontalLines: horizon,
        },
    };

    // Draw graph with default options, overwriting with passed options
    function drawGraph(opts) {

        // Clone the options, so the 'options' variable always keeps intact.
        var o = Flotr._.extend(Flotr._.clone(options), opts || {});

        // Return a new graph.
        return Flotr.draw(
          container,
          d1,
          o
        );
    }

    // Actually draw the graph.
    graph = drawGraph();

    // Hook into the 'flotr:select' event.
    Flotr.EventAdapter.observe(container, 'flotr:select', function (area) {

        // Draw graph with new area
        graph = drawGraph({
            selection: { mode: 'x', fps: 30 },
            bars: {
                show: true,
                barWidth: 0.2,
                //lineWidth: 1,
                //fillColor: {
                //    colors: ['#089938', '#089938'],
                //    start: 'top',
                //    end: 'bottom'
                //},

                horizontal: true,
                fillOpacity: 0.8
            }, yaxis: {
                showLabels: showylabel, min: 0,
                max: maxYvalue,
                noTicks: 10
            },
            xaxis: {
                showLabels: showxlabel, min: 0
            },
            legend: {
                show: showlegend
            },
            grid: {
                tickColor: '#c4c4c4',
                backgroundColor: {
                    colors: [[0, '#fff'], [1, '#e3e3e3']],
                    start: 'top',
                    end: 'bottom'
                },
                verticalLines: vertical,
                horizontalLines: horizon,
            },
            xaxis: { min: area.x1, max: area.x2, showLabels: showxlabel, min: 0 },
            yaxis: {
                min: area.y1, max: area.y2, showLabels: showylabel, min: 0, max: maxYvalue,
                noTicks: 10
            }
        });
    });

    // When graph is clicked, draw the graph with default area.
    Flotr.EventAdapter.observe(container, 'flotr:click', function () { drawGraph(); });
}
var func_Col = function color_gradients(container, arr, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var

      bars = {
          data: [],
          bars: {
              show: true,
              barWidth: 0.8,
              lineWidth: 0,
              fillColor: {
                  colors: ['#ff3112', '#a6c1ff'],
                  start: 'top',
                  end: 'bottom'
              },
              fillOpacity: 0.8
          }
      }, markers = {
          data: [],
          markers: {
              fontsize: 1,

              show: true,
              position: 'ct',
          }
      },
      point,
      graph,
      i;

    point = arr;
    bars.data.push(point);
    markers.data.push(point);


    graph = Flotr.draw(
      container,

      [bars, markers], {
          yaxis: {
              showLabels: false, min: 0,
              max: maxYvalue,
              noTicks: 10
          },
          xaxis: {
              showLabels: false
          },
          grid: {

              tickColor: '#c4c4c4',
              backgroundColor: {
                  colors: ['#e0deff', '#e1f4f7'],
                  start: 'top',
                  end: 'bottom'
              },//'#e0deff', 
              verticalLines: vertical,
              horizontalLines: horizon,
          },
      }
    );
}
var func_Pie = function basic_time(container, arr, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
        graph;

    graph = Flotr.draw(container, arr, {
        HtmlText: false,
        grid: {
            verticalLines: false,
            horizontalLines: false,
            backgroundColor: '#d7deed',
        },
        xaxis: { showLabels: false },
        yaxis: { showLabels: false },
        pie: {
            show: true,
            explode: 6
        },
        mouse: { track: true },
        legend: {
            position: 'se',
        }
    });
}


var func_Line_Report = function basic_time(container, data1, maxYvalue, widthPx, heightPx, horizon, vertical, showlegend, showxlabel, showylabel, showy2label) {
    $(container).width(widthPx);
    $(container).height(heightPx);
    if (widthPx == 0)
        $(container).width('100%')
    if (maxYvalue == 0)
        maxYvalue = null;
    var
      d1 = [],
      start = new Date("2009/01/01 01:00").getTime(),
      options,
      graph,
      i, x, o;



    options = {
        colors: lineColor,
        xaxis: {
            showLabels: showxlabel,
            mode: 'time',
            labelsAngle: 0,
        },
        yaxis: {
            showLabels: showylabel,
            min: 0,
            max: maxYvalue,
            //tickDecimals:10,
            //noTicks: lineTick
        }, y2axis: {
            showLabels: showy2label,
            min: 0,
            max: maxYvalue,
            //tickDecimals:10,
            //noTicks: lineTick
        }, legend: {
            show: showlegend
        }, series: {
            lines: {
                show: true
            },
            points: {
                show: false
            }
        }, grid: {
            //tickColor: '#c4c4c4',
            //backgroundColor: {
            //    colors: ['#dedede', '#dedede'],
            //    start: 'top',
            //    end: 'bottom'
            //},//'#e0deff', 
            verticalLines: vertical,
            horizontalLines: horizon,
        }, lines: {
            lineWidth: 0.5,
        },
        selection: {
            mode: 'x'
        },
        HtmlText: false,
    };

    // Draw graph with default options, overwriting with passed options
    function drawGraph(opts) {

        // Clone the options, so the 'options' variable always keeps intact.
        o = Flotr._.extend(Flotr._.clone(options), opts || {});

        // Return a new graph.
        return Flotr.draw(
          container,
          data1,
          o
        );
    }

    graph = drawGraph();

    Flotr.EventAdapter.observe(container, 'flotr:select', function (area) {
        // Draw selected area
        graph = drawGraph({
            xaxis: { min: area.x1, max: area.x2, mode: 'time', labelsAngle: 0, showLabels: showxlabel },
            yaxis: { min: area.y1, max: area.y2, showLabels: showylabel },
            lines: {
                lineWidth: 0.5,
            }
        });
    });

    // When graph is clicked, draw the graph with default area.
    Flotr.EventAdapter.observe(container, 'flotr:click', function () { graph = drawGraph(); });
};
//var all = [[[0, 5, 99]], [[0, 4, 9]], [[0, 6, 13]], [[0, 7, 28]]];
//var arr = all;
//var nameis = ['C:99%[200/500]', 'D: 5%[100/500]', 'E:15%[10/500]', 'F:15%[10/500]'];
//func_Bar(document.getElementById("grp"), arr, nameis);
