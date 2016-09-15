package foundation.softwaredesign.orbi.model;

import javax.validation.constraints.NotNull;
import javax.xml.bind.annotation.XmlRootElement;
import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Inventory {

    private List<InventoryItem> items = new ArrayList<>();

    public List<InventoryItem> getItems() {
        return items;
    }

    public void setItems(List<InventoryItem> items) {
        this.items = items;
    }
}
