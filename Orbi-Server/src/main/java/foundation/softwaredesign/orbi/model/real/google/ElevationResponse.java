package foundation.softwaredesign.orbi.model.real.google;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement(name = "ElevationResponse")
public class ElevationResponse {

    private String status;

    private List<ElevationResult> result;

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public List<ElevationResult> getResult() {
        return result;
    }

    public void setResult(List<ElevationResult> result) {
        this.result = result;
    }
}
