import 'ol/ol.css';
import Fill from 'ol/style/Fill';
import GeoJSON from 'ol/format/GeoJSON';
import Map from 'ol/Map';
import OSM from 'ol/source/OSM';
import Stroke from 'ol/style/Stroke';
import Style from 'ol/style/Style';
import VectorSource from 'ol/source/Vector';
import View from 'ol/View';
import {Tile as TileLayer, Vector as VectorLayer} from 'ol/layer';

const raster = new TileLayer({
  source: new OSM(),
});

const highlightStyle = new Style({
  fill: new Fill({
    color: 'rgba(255,255,255,0.7)',
  }),
  stroke: new Stroke({
    color: '#3399CC',
    width: 3,
  }),
});

const vector = new VectorLayer({
  source: new VectorSource({
    url: 'https://localhost:44352/api/LABoundary',
    format: new GeoJSON(),
  }),
});

const map = new Map({
    
  layers: [raster, vector],
  target: 'map',
  view: new View({
      center: [51.66694444, 1.01694444],
     
    zoom: 4,
  }),
});

let selected = null;
const status = document.getElementById('status');

map.on('pointermove', function (e) {
  if (selected !== null) {
    selected.setStyle(undefined);
    selected = null;
  }

  map.forEachFeatureAtPixel(e.pixel, function (f) {
    selected = f;
    f.setStyle(highlightStyle);
    return true;
  });

  if (selected) {
    status.innerHTML = '&nbsp;Hovering: ' + selected.get('name');
  } else {
    status.innerHTML = '&nbsp;';
  }
});
