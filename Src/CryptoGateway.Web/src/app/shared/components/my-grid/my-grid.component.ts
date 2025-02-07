import { ChangeDetectorRef, Component, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Pagination } from './pagination.model';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';

@Component({
  selector: 'my-grid',
  templateUrl: './my-grid.component.html',
  styleUrl: './my-grid.component.scss'
})
export class MyGridComponent implements OnInit {
  @Input('showIndex') showIndex: boolean = true;
  @Input('rows') rows: any[] = [];
  @Input('columns') columns: any[] = [];
  @Input('pagination') pagination: Pagination = new Pagination();
  @Input('showAction') showAction: boolean = false;
  @Input('showCustomAction') showCustomAction: boolean = false;
  @Input('showHyperLink') showHyperLink: boolean = false;
  @Input('hyperLinkText') hyperLinkText: string = "";
  @Input('selectable') selectable: boolean = false;
  @Input('responsive') responsive: boolean = true;
  @Input('trackbyPropertyName') trackbyPropertyName: any = undefined;
  @Input('showbuttons') showbuttons: boolean = false;
  @Input('btn1Text') btn1Text: string = "";
  @Input('btn2Text') btn2Text: string = "";
  @Input('copyCondition') copyCondition: boolean = false;
  @Input('copyLongText') copyLongText: boolean = false;

  @Output('onRefresh') onRefresh = new EventEmitter();
  @Output('onEdit') onEdit = new EventEmitter();
  @Output('onDelete') onDelete = new EventEmitter();
  @Output('onSelect') onSelect = new EventEmitter();
  @Output('onHyperLinkClick') onHyperLinkClick = new EventEmitter();
  @Output('onBtn1Click') onBtn1Click = new EventEmitter();
  @Output('onBtn2Click') onBtn2Click = new EventEmitter();
  @Output('onCopyAction') onCopyAction = new EventEmitter();
  @Output('onCopyLongTextAction') onCopyLongTextAction = new EventEmitter();

  @Input() condition: any;
  @Input() btn1condition: any;
  @Input() btn2condition: any;
  @Input('btnFalseConditionText') btnFalseConditionText: string = "";
  //@ViewChild(MatPaginator, { static: false }) paginator!: MatPaginator;//  = new MatPaginator(new MatPaginatorIntl(), ChangeDetectorRef.prototype);
  
  rowSelectedIndex!: number;
  paginationMaxSize!: number;

  constructor() { }

  ngOnInit() {
    this.analyzeScreenSize();
    // console.log(this.paginator, this.pagination);
    // if (this.paginator) {
    //   // this.paginator.pageIndex = this.pagination.currentPage;
    //   // this.paginator.pageSize = this.pagination.pageSize;
    //   // this.paginator.length = this.pagination.totalCount;
    //   this.paginator.firstPage();
    // }
  }

  @HostListener("window:resize", []) analyzeScreenSize() {
    // lg (for laptops and desktops - screens equal to or greater than 1200px wide)
    // md (for small laptops - screens equal to or greater than 992px wide)
    // sm (for tablets - screens equal to or greater than 768px wide)
    // xs (for phones - screens less than 768px wide)
  
    if (window.innerWidth >= 1200) {
      this.paginationMaxSize = 10; // lg
    } else if (window.innerWidth >= 992) {
      this.paginationMaxSize = 7;//md
    } else if (window.innerWidth  >= 768) {
      this.paginationMaxSize = 5;//sm
    } else if (window.innerWidth < 768) {
      this.paginationMaxSize = 2;//xs
    }
  }

  refresh($event: any) {
    this.onRefresh.emit($event);
  }

  edit($event: any) {
    this.onEdit.emit($event);
  }

  delete($event: any) {
    this.onDelete.emit($event);
  }

  onClick(row: any, index: number) {
    if (this.selectable)
    {
      this.rowSelectedIndex = index;
      this.onSelect.emit(row);
    }
    //console.log('onClick()', index, row);
  }

  hyperLinkClick($event: any) {
    this.onHyperLinkClick.emit($event);
  }

  identify(index: any, item: { [x: string]: any; }) {
    if (this.trackbyPropertyName != undefined) {
      //console.log(item[this.trackbyPropertyName]);
      return item[this.trackbyPropertyName]; 
    }
 }

 btn1Click($event: any) {
  this.onBtn1Click.emit($event)
 }

 btn2Click($event: any) {
  this.onBtn2Click.emit($event)
 }

 removeParentheses($event: string) {
  return $event.replace(/[()]/g, '');
  }
  
  copyAction($event: any) {
    this.onCopyAction.emit($event);
  }

  copyLongTextAction($text: string) {
    this.onCopyLongTextAction.emit($text);
  }
}
